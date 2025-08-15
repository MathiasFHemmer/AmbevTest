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
}