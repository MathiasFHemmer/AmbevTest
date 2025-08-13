using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public sealed class CreateSaleHandlerTests
{
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _handler = new CreateSaleHandler();
    }
}