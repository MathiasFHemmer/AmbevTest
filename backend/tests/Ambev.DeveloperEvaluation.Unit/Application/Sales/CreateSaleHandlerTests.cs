using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;
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
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For <ISaleRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _handler = new CreateSaleHandler(_customerRepository, _branchRepository, _saleRepository);
    }

    [Fact(DisplayName = "Given valid sale data When creating sale Then returns sale")]
    public async Task Handle_ValidRequest_ReturnsSale()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.Generate();
        var sale = SaleTestData
            .Generate()
            .WithSaleNumber(command.SaleNumber)
            .WithBranchId(command.BranchId)
            .WithCustomerId(command.CustomerId);
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

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _customerRepository.GetById(command.CustomerId).Returns(customer);
        _branchRepository.GetById(command.BranchId).Returns(branch);

        // Act
        var expectedSale = await _handler.Handle(command, CancellationToken.None);

        // Assert
        expectedSale.SaleNumber.Should().Be(command.SaleNumber);
        expectedSale.CustomerId.Should().Be(customer.Id);
        expectedSale.CustomerName.Should().Be(customer.Name);
        expectedSale.BranchId.Should().Be(branch.Id);
        expectedSale.BranchName.Should().Be(branch.Name);
    }
}