namespace CustomerManagement.Application.Contacts.Common;

public abstract record PagedRequest
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}