using Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="CompleteSaleHandler"/> class.
/// </summary>
public sealed class CompleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly CompleteSaleHandler _handler;

    public CompleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new CompleteSaleHandler(_saleRepository);
    }

    [Fact(DisplayName = "Should complete sale when it exists and is valid")]
    public async Task Handle_ValidSale_CompletesSale()
    {
        // Arrange
        var sale = SaleTestData.Generate()
            .WithStatus(SaleStatus.Pending);

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        var command = new CompleteSaleCommand { SaleId = sale.Id };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        sale.Status.Should().Be(SaleStatus.Completed);
        sale.CompletedAt.Should().NotBeNull();
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Should throw NotFoundException when sale does not exist")]
    public async Task Handle_NonExistentSale_ThrowsNotFoundException()
    {
        // Arrange
        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        var command = new CompleteSaleCommand { SaleId = Guid.NewGuid() };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        await _saleRepository.Received(1).GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Should do nothing when sale is already completed")]
    public async Task Handle_AlreadyCompletedSale_ThrowsDomainException()
    {
        // Arrange
        var sale = SaleTestData.Generate()
            .WithStatus(SaleStatus.Completed);

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        var command = new CompleteSaleCommand { SaleId = sale.Id };

        // Act 
        await _handler.Handle(command, CancellationToken.None);
        // Assert
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Should throw DomainException when sale is cancelled")]
    public async Task Handle_CancelledSale_ThrowsDomainException()
    {
        // Arrange
        var sale = SaleTestData.Generate()
            .WithStatus(SaleStatus.Cancelled);

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        var command = new CompleteSaleCommand { SaleId = sale.Id };

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
