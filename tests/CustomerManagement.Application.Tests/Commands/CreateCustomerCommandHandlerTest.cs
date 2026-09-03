using AutoMapper;
using CustomerManagement.Application.Mapping;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using FluentAssertions;
using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Application.Commands.Tests;

public class CreateCustomerCommandHandlerTest
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly CreateCustomerCommandHandler _handler;


    public CreateCustomerCommandHandlerTest()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mapper = new MapperConfiguration(
            cfg => cfg.AddProfile<CustomerProfile>(),
            NullLoggerFactory.Instance).CreateMapper();

        _handler = new CreateCustomerCommandHandler(_customerRepository, _mapper);
    }

    [Fact]
    public async Task Create_CustomerNonEmailDuplication_Test()
    {
        var currentDate = DateTime.UtcNow;
        var customer = new Customer
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "JohnDoe@maildrop.cc",
            PhoneNumber = "0811238765",
            CreatedDate = currentDate
        };
        _customerRepository.CheckIfEmailExistsAsync(customer.Email, null, CancellationToken.None)
            .Returns(false);

        var command = new CreateCustomerCommand(customer.FirstName, customer.LastName, customer.Email, customer.PhoneNumber);
        var result = await _handler.Handle(command, CancellationToken.None);

        await _customerRepository.Received(1).AddAsync(
            Arg.Is<Customer>(c => c.Email == command.Email && c.FirstName == command.FirstName),
            Arg.Any<CancellationToken>());

        result.Should().NotBeNull();
        result.FirstName.Should().Be(customer.FirstName);
        result.LastName.Should().Be(customer.LastName);
        result.Email.Should().Be(customer.Email);
        result.PhoneNumber.Should().Be(customer.PhoneNumber);
        result.CreatedDate.Should().BeOnOrAfter(customer.CreatedDate);
    }

    [Fact]
    public async Task Create_CustomerEmailDuplication_Test()
    {
        var currentDate = DateTime.UtcNow;
        var customer = new Customer
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "JohnDoe@maildrop.cc",
            PhoneNumber = "0811238765",
            CreatedDate = currentDate
        };
        _customerRepository.CheckIfEmailExistsAsync(customer.Email, null, CancellationToken.None)
            .Returns(true);

        var command = new CreateCustomerCommand(customer.FirstName, customer.LastName, customer.Email, customer.PhoneNumber);
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DuplicateEmailException>();
    }
}