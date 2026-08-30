namespace CustomerManagement.Domain.Exceptions;

public class CustomerNotFoundException(Guid id)
    : Exception($"Customer with Id {id} was not found")
{
    public Guid Id { get; } = id;
}