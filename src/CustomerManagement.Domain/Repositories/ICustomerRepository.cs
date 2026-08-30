using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Domain.Repositories;

public interface ICustomerRepository: IRepository<Customer>
{
    public Task<bool> CheckIfEmailExistsAsync(string email, Guid? excludeId, CancellationToken cancellationToken = default);
}