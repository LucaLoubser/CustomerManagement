using AutoMapper;
using CustomerManagement.Application.Mapping;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using FluentAssertions;
using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Application.Queries.Tests;

public class GetCustomerByIdQueryHandlerTest
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly GetCustomerByIdQueryHandler _handler;

    public GetCustomerByIdQueryHandlerTest()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mapper = new MapperConfiguration(
            cfg => cfg.AddProfile<CustomerProfile>(),
            NullLoggerFactory.Instance).CreateMapper();

        _handler = new GetCustomerByIdQueryHandler(_customerRepository, _mapper);
    }

    [Fact]
    public async Task Get_Existing_CustomerById_Test()
    {
        var id = new Guid("d12a3fb1-5d6b-4f7a-b9b8-3c6ce5f4f529");
        var customer = new Customer
        {
            Id = id,
            FirstName = "John",
            LastName = "Doe",
            Email = "JohnDoe@maildrop.cc",
            PhoneNumber = "0811238765",
            CreatedDate = DateTime.UtcNow
        };
        _customerRepository.GetByIdAsync(id).Returns(customer);

        var query = new GetCustomerByIdQuery(id);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(customer.Id);
        result.FirstName.Should().Be(customer.FirstName);
        result.LastName.Should().Be(customer.LastName);
        result.Email.Should().Be(customer.Email);
        result.PhoneNumber.Should().Be(customer.PhoneNumber);
        result.CreatedDate.Should().Be(customer.CreatedDate);
    }

    [Fact]
    public async Task TryGet_NonExistingCustomerById_Test()
    {
        var invalidId = new Guid("6c1d6156-d964-4102-9084-30d745a57c0c");

        var query = new GetCustomerByIdQuery(invalidId);
        Func<Task> act = () => _handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<CustomerNotFoundException>();
    }
}