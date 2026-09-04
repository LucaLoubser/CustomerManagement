using CustomerManagement.Application.Contracts.Common;

namespace CustomerManagement.Application.Contracts.Requests;

public record GetPagedCustomerRequestDto : PagedRequest
{
    public string? Filter { get; init; }
}