using Ambev.DeveloperEvaluation.Application.Sales.ListSaleItems;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Pagination;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public sealed class ListSaleItemsHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ListSaleItemsHandler _handler;

    public ListSaleItemsHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ListSaleItemsProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new ListSaleItemsHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "Should return a paginated list of sale items")]
    public async Task Handle_WithSaleItems_ReturnsMappedList()
    {
        // Arrange
        var saleItems = new List<SaleItem>
        {
            new SaleItem(Guid.NewGuid(), "Product A", 2, 50m),
            new SaleItem(Guid.NewGuid(), "Product B", 1, 100m)
        };
        var paginated = new PaginatedList<SaleItem>(saleItems, 2, 1, 10);

        _saleRepository.ListSaleItemsBySaleId(Arg.Any<Guid>(), Arg.Any<PaginateRequest>(), Arg.Any<CancellationToken>())
            .Returns(paginated);

        var command = ListSaleItemsHandlerTestData.Generate();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Count().Should().Be(2);
        result.TotalCount.Should().Be(2);
        result.CurrentPage.Should().Be(1);
        result.PageSize.Should().Be(10);

        // Check computed fields are mapped correctly
        var firstItem = result.Items.First();
        firstItem.TotalPrice.Should().Be(saleItems[0].TotalPrice);
        firstItem.DiscountAmount.Should().Be(saleItems[0].DiscountAmount);
    }

    [Fact(DisplayName = "Should return an empty list when no sale items exist")]
    public async Task Handle_NoSaleItems_ReturnsEmptyList()
    {
        // Arrange
        var paginated = new PaginatedList<SaleItem>(new List<SaleItem>(), 0, 1, 10);

        _saleRepository.ListSaleItemsBySaleId(Arg.Any<Guid>(), Arg.Any<PaginateRequest>(), Arg.Any<CancellationToken>())
            .Returns(paginated);

        var command = ListSaleItemsHandlerTestData.Generate();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }
}
