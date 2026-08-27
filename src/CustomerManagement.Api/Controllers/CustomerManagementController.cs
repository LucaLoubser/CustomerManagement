using CustomerManagement.Application.Commands;
using CustomerManagement.Application.Contracts.Customer;
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
}