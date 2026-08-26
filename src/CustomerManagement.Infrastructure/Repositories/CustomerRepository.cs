using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Repositories;

namespace CustomerManagement.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(CustomerManagementDbContext dbContext) : base(dbContext)
    {
    }
}