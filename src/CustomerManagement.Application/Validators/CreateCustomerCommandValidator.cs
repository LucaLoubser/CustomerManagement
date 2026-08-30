using CustomerManagement.Application.Commands;
using FluentValidation;

namespace CustomerManagement.Application.Validators;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName cannot be empty.")
            .NotNull().WithMessage("FirstName is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName cannot be empty.")
            .NotNull().WithMessage("LastName is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .NotNull().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please enter a valid email address.");

        RuleFor(x => x.PhoneNumber)
           .Matches(@"^\+?[0-9\s\-()]{7,20}$")
           .WithMessage("Please enter a valid phone number.");
    }
}