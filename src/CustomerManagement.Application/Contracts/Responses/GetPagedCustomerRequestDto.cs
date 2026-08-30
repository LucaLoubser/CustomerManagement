using CustomerManagement.Application.Contacts.Common;

namespace CustomerManagement.Application.Contacts.Requests;

public record GetPagedCustomerRequestDto : PagedRequest
{
    public string? Filter { get; init; }
}