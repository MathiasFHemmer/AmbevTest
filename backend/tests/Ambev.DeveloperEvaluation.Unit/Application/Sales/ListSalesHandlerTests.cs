using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Pagination;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="ListSalesHandler"/> class.
/// </summary>
public sealed class ListSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ListSalesHandler _handler;

    public ListSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new ListSalesHandler(_saleRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedResult_WhenSalesExist()
    {
        // Arrange
        var command = ListSalesHandlerTestData.Generate();
        var fakeSales = new PaginatedList<Sale>([SaleTestData.Generate()], 1, 1, 10);
        var fakeSaleResultEntry = new ListSaleResultEntry();

        var expectedResult = new ListSaleResult([fakeSaleResultEntry], fakeSales.TotalCount, fakeSales.CurrentPage, fakeSales.PageSize);

        _saleRepository.ListSales(command.Pagination, Arg.Any<CancellationToken>())
            .Returns(fakeSales);

        _mapper.Map<ListSaleResult>(fakeSales).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleRepository.Received(1)
            .ListSales(command.Pagination, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyData_WhenNoSalesExist()
    {
        // Arrange
        var command = ListSalesHandlerTestData.Generate();
        var emptySales = new PaginatedList<Sale>([], 0, 1, 10);

        var expectedResult = new ListSaleResult([], emptySales.TotalCount, emptySales.CurrentPage, emptySales.PageSize);

        _saleRepository.ListSales(command.Pagination, Arg.Any<CancellationToken>())
            .Returns(emptySales);

        _mapper.Map<ListSaleResult>(emptySales).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.TotalCount.Should().Be(0);
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldPropagateException_WhenRepositoryThrows()
    {
        // Arrange
        var command = ListSalesHandlerTestData.Generate();
        _saleRepository.ListSales(command.Pagination, Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception());

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}