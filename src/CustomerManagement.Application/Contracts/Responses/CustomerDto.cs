namespace CustomerManagement.Application.Contracts.Responses;

public record CustomerDto
{
    public Guid Id {get; set;}
    public required string FirstName {get; set;}
    public required string LastName {get; set;}
    public required string Email {get; set;}
    public string? PhoneNumber {get; set;}
    public DateTime CreatedDate {get; set;}
}