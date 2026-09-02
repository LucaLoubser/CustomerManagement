using CustomerManagement.Application.Commands;
using CustomerManagement.Application.Contacts.Common;
using CustomerManagement.Application.Contacts.Requests;
using CustomerManagement.Application.Contracts.Customer;
using CustomerManagement.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerManagementController(
    IMediator mediator
) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto?>> CreateAsync(
        [FromBody] CreateCustomerRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateCustomerCommand(request.FirstName, request.LastName, request.Email, request.PhoneNumber), cancellationToken);
        return CreatedAtRoute(nameof(GetByIdAsync), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto?>> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateCustomerRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateCustomerCommand(id, request.FirstName, request.LastName, request.Email, request.PhoneNumber), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync(
        [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCustomerCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("{id}", Name = nameof(GetByIdAsync))]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto?>> GetByIdAsync(
        [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustomerByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<CustomerDto>?>> GetPagedListAsync(
        [FromQuery] GetPagedCustomerRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPagedCustomerQuery(request.PageNumber, request.PageSize, request.Filter), cancellationToken);
        return Ok(result);
    }
}