using Ambev.DeveloperEvaluation.Application.Sales.SetSaleItemQuantity;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public sealed class SetSaleItemQuantityHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly SetSaleItemQuantityHandler _handler;

    public SetSaleItemQuantityHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new SetSaleItemQuantityHandler(_saleRepository);
    }

    [Fact(DisplayName = "Should set item quantity when sale and item exist")]
    public async Task Handle_ValidSaleAndItem_UpdatesQuantity()
    {
        // Arrange
        var newQuantity = 5u;
        var saleItem = SaleItemTestData.Generate();
        var sale = SaleTestData.Generate().WithSaleItem(saleItem);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);

        var command = SetSaleItemQuantityHandlerTestData.Generate()
            .WithSaleId(sale.Id)
            .WithProductId(saleItem.ProductId)
            .WithNewQuantity(newQuantity);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        sale.GetItem(saleItem.ProductId)?.Quantity.Should().Be(newQuantity);
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Should throw when sale does not exist")]
    public async Task Handle_NonExistentSale_ThrowsDomainException()
    {
        // Arrange
        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Sale?)null);

        var command = SetSaleItemQuantityHandlerTestData.Generate();

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Should throw when sale item does not exist")]
    public async Task Handle_NonExistentSaleItem_ThrowsDomainException()
    {
        // Arrange
        var sale = SaleTestData.Generate();
        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);

        var command = SetSaleItemQuantityHandlerTestData.Generate()
            .WithSaleId(sale.Id)
            .WithProductId(Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
