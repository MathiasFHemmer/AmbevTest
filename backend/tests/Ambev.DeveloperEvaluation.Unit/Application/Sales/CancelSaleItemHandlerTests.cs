using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Sales;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="CancelSaleItemHandler"/> class.
/// </summary>
public sealed class CancelSaleItemHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly CancelSaleItemHandler _handler;

    public CancelSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new CancelSaleItemHandler(_saleRepository);
    }

    [Fact(DisplayName = "Should cancel sale item when sale and item exist and both are in valid status")]
    public async Task Handle_ValidSaleAndItem_CancelItem()
    {
        // Arrange
        var saleItem = SaleItemTestData.Generate();
        var sale = SaleTestData.Generate()
            .WithSaleItem(saleItem);

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(sale);

        var command = CancelSaleItemHandlerTestData.Generate()
            .WithSaleId(sale.Id)
            .WithProductId(saleItem.ProductId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        sale.GetItem(saleItem.ProductId).Should().BeNull();
        sale.SaleItems.Count.Should().Be(0);
    }

    [Fact(DisplayName = "Should throw when sale exists but its cancelled")]
    public async Task Handle_SaleIsCancelled_ThrowsNotFoundException()
    {
        // Arrange
        var saleItem = SaleItemTestData.Generate();
        var sale = SaleTestData.Generate()
            .WithSaleItem(saleItem)
            .WithStatus(SaleStatus.Cancelled);

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(sale);

        var command = CancelSaleItemHandlerTestData.Generate()
            .WithSaleId(sale.Id)
            .WithProductId(saleItem.ProductId);

        // Act and Assert
        await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Should throw when sale does not exist")]
    public async Task Handle_NonExistentSale_ThrowsNotFoundException()
    {
        // Arrange
        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Sale?)null);

        var command = CancelSaleItemHandlerTestData.Generate();

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Should throw when sale item does not exist")]
    public async Task Handle_NonExistentSaleItem_ThrowsNotFoundException()
    {
        // Arrange
        var sale = SaleTestData.Generate();

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(sale);

        var command = CancelSaleItemHandlerTestData.Generate()
            .WithSaleId(sale.Id);

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        await _saleRepository.Received(1).GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Should throw when sale item exists but its cancelled already")]
    public async Task Handle_AlreadyCancelledItem_ThrowsNotFoundException()
    {
        // Arrange
        var saleItem = SaleItemTestData.Generate()
            .WithStatus(SaleItemStatus.Cancelled);

        var sale = SaleTestData.Generate()
            .WithSaleItem(saleItem);

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(sale);

        var command = CancelSaleItemHandlerTestData.Generate()
            .WithSaleId(sale.Id)
            .WithProductId(saleItem.ProductId);

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
