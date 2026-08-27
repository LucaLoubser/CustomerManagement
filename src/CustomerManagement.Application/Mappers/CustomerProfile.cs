using AutoMapper;
using CustomerManagement.Application.Commands;
using CustomerManagement.Application.Contracts.Customer;
using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Application.Mapping;

public sealed class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<CreateCustomerCommand, Customer>();
    }
}