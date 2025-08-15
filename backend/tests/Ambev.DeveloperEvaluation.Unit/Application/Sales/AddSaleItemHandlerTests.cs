using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Policies;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Sales;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="AddSaleItemHandlerTests"/> class.
/// </summary>
public sealed class AddSaleItemHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly AddSaleItemHandler _handler;

    public AddSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new AddSaleItemHandler(_saleRepository);
    }

    [Fact(DisplayName = "Given valid sale item data When adding sale item Then returns sale item")]
    public async Task Handle_ValidRequest_ReturnsSaleItem()
    {
        // Arrange
        var sale = SaleTestData.Generate();
        var command = AddSaleItemHandlerTestsData.Generate()
            .WithSaleId(sale.Id);

        var productName = "Placeholder";

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(sale);
        // Act
        var expectedSaleItem = await _handler.Handle(command, CancellationToken.None);

        // Assert
        expectedSaleItem.ProductName.Should().BeEquivalentTo(productName);
    }

    [Fact(DisplayName = "Given active sale with item present When adding the same product Then throws exception")]
    public async Task Handler_AddDuplicateItem_ThrowsException()
    {
        // Arrange
        var sale = SaleTestData.Generate();
        var saleIem = SaleItemTestData.Generate();
        var command = AddSaleItemHandlerTestsData.Generate()
            .WithSaleId(sale.Id)
            .WithProductId(saleIem.ProductId);

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(sale);
        sale.AddItem(command.ProductId, "Placeholder", command.Quantity, command.UnitPrice);
        // Act and Assert
        await Assert.ThrowsAsync<DuplicateItemInSaleException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given active sale When adding products Then must recalculate the sale total")]
    public async Task Handler_AddNewProduct_RecalculatesSaleTotal()
    {
        // Arrange
        var sale = SaleTestData.Generate();

        var unitPrice = 10m;
        var quantity = 1u;
        var expectedTotal = unitPrice * quantity * 2;

        var commandSaleItem1 = AddSaleItemHandlerTestsData.Generate()
            .WithSaleId(sale.Id)
            .WithQuantity(quantity)
            .WithUnitPrice(unitPrice);

        var commandSaleItem2 = AddSaleItemHandlerTestsData.Generate()
            .WithSaleId(sale.Id)
            .WithQuantity(quantity)
            .WithUnitPrice(unitPrice);

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        sale.AddItem(commandSaleItem1.ProductId, "Placeholder", commandSaleItem1.Quantity, commandSaleItem1.UnitPrice);
        sale.AddItem(commandSaleItem2.ProductId, "Placeholder", commandSaleItem2.Quantity, commandSaleItem2.UnitPrice);

        // Assert
        sale.SaleItems.Count.Should().Be(2);
        sale.TotalAmount.Should().Be(expectedTotal);
    }

    [Fact(DisplayName = "Given active sale with cancelled item When adding another product Then cancelled item must not impact total")]
    public void Handler_AddNewProductWithCancelledItem_RecalculatesSaleTotalIgnoresCancelledItem()
    {
        // Arrange
        var unitPrice = 10m;
        var quantity = 1u;
        var expectedTotal = unitPrice * quantity;

        var saleIem = SaleItemTestData.Generate()
            .WithQuantity(quantity)
            .WithUnitPrice(unitPrice)
            .WithStatus(SaleItemStatus.Cancelled);

        var sale = SaleTestData.Generate()
            .WithSaleItem(saleIem);

        var commandSaleItem = AddSaleItemHandlerTestsData.Generate()
            .WithSaleId(sale.Id)
            .WithQuantity(quantity)
            .WithUnitPrice(unitPrice);

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        sale.AddItem(commandSaleItem.ProductId, "Placeholder", commandSaleItem.Quantity, commandSaleItem.UnitPrice);

        // Assert
        sale.SaleItems.Count.Should().Be(1);
        sale.TotalAmount.Should().Be(expectedTotal);
    }

    [Fact(DisplayName = "Given active sale with unknown status item When adding another product Then cancelled item must not impact total")]
    public void Handler_AddNewProductWithUnknownStatusItem_RecalculatesSaleTotalIgnoresUnknownStatusItem()
    {
        // Arrange
        var unitPrice = 10m;
        var quantity = 1u;
        var expectedTotal = unitPrice * quantity;

        var saleIem = SaleItemTestData.Generate()
            .WithQuantity(quantity)
            .WithUnitPrice(unitPrice)
            .WithStatus(SaleItemStatus.Unknown);

        var sale = SaleTestData.Generate()
            .WithSaleItem(saleIem);

        var commandSaleItem = AddSaleItemHandlerTestsData.Generate()
            .WithSaleId(sale.Id)
            .WithQuantity(quantity)
            .WithUnitPrice(unitPrice);

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        sale.AddItem(commandSaleItem.ProductId, "Placeholder", commandSaleItem.Quantity, commandSaleItem.UnitPrice);

        // Assert
        sale.SaleItems.Count.Should().Be(1);
        sale.TotalAmount.Should().Be(expectedTotal);
    }
    
    [Fact(DisplayName = "Given active sale with discount policy  When adding new product Then must apply discount policy")]
    public void Handler_AddNewItemWithDiscountPolicy_AppliesDiscountPolicy()
    {
        // Arrange
        var discount = 0.3m; // 30%
        var discountPolicy = Substitute.For<IDiscountPolicy>();
        discountPolicy.GetDiscount(Arg.Any<SaleItem>()).Returns(discount);

        var unitPrice = 10m;
        var quantity = 1u;
        var expectedTotal = unitPrice * quantity * (1-discount);

        var saleIem = SaleItemTestData.Generate()
            .WithQuantity(quantity)
            .WithUnitPrice(unitPrice)
            .WithStatus(SaleItemStatus.Unknown);

        var sale = SaleTestData.Generate()
            .WithDiscountPolicy(discountPolicy)
            .WithSaleItem(saleIem);

        var commandSaleItem = AddSaleItemHandlerTestsData.Generate()
            .WithSaleId(sale.Id)
            .WithQuantity(quantity)
            .WithUnitPrice(unitPrice);

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        sale.AddItem(commandSaleItem.ProductId, "Placeholder", commandSaleItem.Quantity, commandSaleItem.UnitPrice);

        // Assert
        sale.SaleItems.Count.Should().Be(1);
        sale.TotalAmount.Should().Be(expectedTotal);
    }
}