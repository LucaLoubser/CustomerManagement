using Microsoft.EntityFrameworkCore;
using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Infrastructure;

public class CustomerManagementDbContext(DbContextOptions<CustomerManagementDbContext> options) : DbContext(options)
{
    public virtual DbSet<Customer> Customers { get; set; }
}