using AutoMapper;
using CustomerManagement.Application.Contracts.Responses;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.Repositories;
using MediatR;

namespace CustomerManagement.Application.Commands;

public record CreateCustomerCommand(string FirstName, string LastName, string Email, string? PhoneNumber)
    : IRequest<CustomerDto>;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var duplicateEmailExists = await _customerRepository
            .CheckIfEmailExistsAsync(command.Email, null, cancellationToken);
        if (duplicateEmailExists)
        {
            throw new DuplicateEmailException(command.Email);
        }

        var customer = _mapper.Map<Customer>(command);
        customer.CreatedDate = DateTime.UtcNow;

        await _customerRepository
            .AddAsync(customer, cancellationToken);
        return _mapper.Map<CustomerDto>(customer);
    }
}