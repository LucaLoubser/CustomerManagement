using CustomerManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerManagement.Infrastructure.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.FirstName).HasMaxLength(32).IsRequired();
        builder.Property(c => c.LastName).HasMaxLength(32).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(256).IsRequired();
        builder.Property(c => c.PhoneNumber).HasMaxLength(32);

        builder.HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("IX_Customers_Email");
    }
}
