using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    private readonly CustomerManagementDbContext _dbContext;

    public CustomerRepository(CustomerManagementDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CheckIfEmailExistsAsync(string email, Guid? excludeId, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Customers.AsNoTracking();
        if(excludeId != null){
            query = query.Where(c => c.Id != excludeId);
        }
        return await query.AnyAsync(x => x.Email == email, cancellationToken);
    }

    protected override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sql
            && (sql.Number == 2601 || sql.Number == 2627)
            && sql.Message.Contains("IX_Customers_Email"))
        {
            var email = ex.Entries.Select(e => e.Entity).OfType<Customer>().FirstOrDefault()?.Email;
            throw new DuplicateEmailException(email ?? "unknown");
        }
    }
}