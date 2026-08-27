namespace CustomerManagement.Domain.Exceptions;

public class DuplicateEmailException(string email)
    : Exception($"A customer with email {email} already exists")
{
    public string Email { get; } = email;
}