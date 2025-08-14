using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Sales;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public sealed class CreateSaleHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _handler = new CreateSaleHandler(_customerRepository, _branchRepository);
    }

    [Fact(DisplayName = "Given valid sale data When creating sale Then returns sale")]
    public async Task Handle_ValidRequest_ReturnsSale()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.Generate();
        // TODO:
        // Move entity creation to dedicated Faker generators
        var customer = new Customer
        {
            Id = command.CustomerId,
            Name = "Alice"
        };
        var branch = new Branch
        {
            Id = command.BranchId,
            Name = "Main Branch"
        };

        _customerRepository.GetById(command.CustomerId).Returns(customer);
        _branchRepository.GetById(command.BranchId).Returns(branch);

        // Act
        var sale = await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.SaleNumber.Should().Be(command.SaleNumber);
        sale.CustomerId.Should().Be(customer.Id);
        sale.CustomerName.Should().Be(customer.Name);
        sale.BranchId.Should().Be(branch.Id);
        sale.BranchName.Should().Be(branch.Name);
    }
}