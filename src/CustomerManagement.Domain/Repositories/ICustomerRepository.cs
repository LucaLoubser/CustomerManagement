using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Domain.Repositories;

public interface ICustomerRepository: IRepository<Customer>
{
    public Task<bool> CheckIfEmailExistsAsync(string email, Guid? excludeId, CancellationToken cancellationToken = default);
    public new Task<Customer> UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
    public Task<List<Customer>> GetListAsync(int pageNumber, int pageSize, string? filter, CancellationToken cancellationToken = default);
    public Task<long> GetCountAsync(string? filter, CancellationToken cancellationToken = default);
}