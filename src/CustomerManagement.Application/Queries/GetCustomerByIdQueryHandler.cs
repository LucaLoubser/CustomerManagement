using AutoMapper;
using CustomerManagement.Application.Contracts.Responses;
using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.Repositories;
using MediatR;

namespace CustomerManagement.Application.Queries;

public record GetCustomerByIdQuery(Guid Id)
    : IRequest<CustomerDto>;

public class GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new CustomerNotFoundException(query.Id);

        return _mapper.Map<CustomerDto>(customer);
    }
}