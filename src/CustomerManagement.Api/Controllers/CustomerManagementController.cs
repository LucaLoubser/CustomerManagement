using CustomerManagement.Application.Commands;
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
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateCustomerRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateCustomerCommand(request.FirstName, request.LastName, request.Email, request.PhoneNumber), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateCustomerRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateCustomerCommand(id, request.FirstName, request.LastName, request.Email, request.PhoneNumber), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCustomerCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustomerByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetPagedListAsync(
        [FromQuery] GetPagedCustomerRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPagedCustomerQuery(request.PageNumber, request.PageSize, request.Filter), cancellationToken);
        return Ok(result);
    }
}