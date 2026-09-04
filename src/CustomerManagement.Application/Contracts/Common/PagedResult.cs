namespace CustomerManagement.Application.Contracts.Common;

public record PagedResult<T>(IReadOnlyList<T> Items, int PageNumber, int PageSize, long TotalCount)
{
}
