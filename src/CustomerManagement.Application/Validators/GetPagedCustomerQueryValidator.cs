using CustomerManagement.Application.Queries;
using FluentValidation;

namespace CustomerManagement.Application.Validators;

public class GetPagedCustomerQueryValidator : AbstractValidator<GetPagedCustomerQuery>
{
    public GetPagedCustomerQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be larger then zero.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
    }
}