namespace CustomerManagement.Application.Contracts.Customer;

public record CreateCustomerRequestDto
{
    public required string FirstName {get; set;}
    public required string LastName {get; set;}
    public required string Email {get; set;}
    public string? PhoneNumber {get; set;}
}