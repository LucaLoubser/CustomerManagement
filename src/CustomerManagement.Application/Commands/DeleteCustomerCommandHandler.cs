using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.Repositories;
using MediatR;

namespace CustomerManagement.Application.Commands;

public record DeleteCustomerCommand(Guid Id)
    : IRequest;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        var _ = await _customerRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new CustomerNotFoundException(command.Id);

        await _customerRepository.DeleteByIdAsync(command.Id, cancellationToken);
    }
}