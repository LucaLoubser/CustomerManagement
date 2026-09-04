using AutoMapper;
using CustomerManagement.Application.Contracts.Responses;
using CustomerManagement.Domain.Repositories;
using MediatR;
using CustomerManagement.Application.Contracts.Common;

namespace CustomerManagement.Application.Queries;

public record GetPagedCustomerQuery(int PageNumber, int PageSize, string? Filter)
    : IRequest<PagedResult<CustomerDto>>;

public class GetPagedCustomerQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    : IRequestHandler<GetPagedCustomerQuery, PagedResult<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedResult<CustomerDto>> Handle(GetPagedCustomerQuery query, CancellationToken cancellationToken)
    {
        var items = await _customerRepository.GetListAsync(query.PageNumber, query.PageSize, query.Filter, cancellationToken);
        var total = await _customerRepository.GetCountAsync(query.Filter, cancellationToken);

        return new PagedResult<CustomerDto>(_mapper.Map<List<CustomerDto>>(items), query.PageNumber, query.PageSize, total);
    }
}