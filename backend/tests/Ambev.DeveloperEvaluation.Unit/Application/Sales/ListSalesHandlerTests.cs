using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Pagination;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Sales;
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
        var fakeSales = PaginatedList<Sale>.Create([SaleTestData.Generate()], 1, 1, 10);

        var expectedResult = new ListSaleResult { Data = fakeSales };

        _saleRepository.ListSales(command.Pagination, Arg.Any<CancellationToken>())
            .Returns(fakeSales);

        _mapper.Map<ListSaleResult>(fakeSales).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        await _saleRepository.Received(1)
            .ListSales(command.Pagination, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<ListSaleResult>(fakeSales);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyData_WhenNoSalesExist()
    {
        // Arrange
        var command = ListSalesHandlerTestData.Generate();
        var emptySales = PaginatedList<Sale>.Create([], 0, 0, 1);

        var expectedResult = new ListSaleResult { Data = emptySales };

        _saleRepository.ListSales(command.Pagination, Arg.Any<CancellationToken>())
            .Returns(emptySales);

        _mapper.Map<ListSaleResult>(emptySales).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Data.TotalCount.Should().Be(0);
        result.Data.Items.Should().BeEmpty();
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