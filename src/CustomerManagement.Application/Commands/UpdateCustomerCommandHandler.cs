using AutoMapper;
using CustomerManagement.Application.Contracts.Customer;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.Repositories;
using MediatR;

namespace CustomerManagement.Application.Commands;

public record UpdateCustomerCommand(Guid Id,string FirstName, string LastName, string Email, string? PhoneNumber)
    : IRequest<CustomerDto>;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new CustomerNotFoundException(command.Id);

        var duplicateEmailExists = await _customerRepository.CheckIfEmailExistsAsync(command.Email, command.Id, cancellationToken);
        if (duplicateEmailExists)
        {
            throw new DuplicateEmailException(command.Email);
        }

        customer.FirstName = command.FirstName;
        customer.LastName = command.LastName;
        customer.Email = command.Email;
        customer.PhoneNumber = command.PhoneNumber;

        await _customerRepository.UpdateAsync(customer, cancellationToken);
        return _mapper.Map<CustomerDto>(customer);

    }
}