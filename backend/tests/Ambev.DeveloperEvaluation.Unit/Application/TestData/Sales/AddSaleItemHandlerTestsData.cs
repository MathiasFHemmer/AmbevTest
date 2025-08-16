using Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Sales;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class AddSaleItemHandlerTestsData
{
    /// <summary>
    /// Configures the Faker to generate valid normalized AddSaleItem command.
    /// The generated command will have valid:
    /// - SaleId (Valid random Guid)
    /// - ProductId (Valid random Guid)
    /// - Quantity (1u)
    /// - UnitPrice (1m)
    /// </summary>
    private static readonly Faker<AddSaleItemCommand> AddSaleItemCommandFaker = new Faker<AddSaleItemCommand>()
        .RuleFor(sale => sale.SaleId, faker => faker.Random.Guid())
        .RuleFor(sale => sale.ProductId, faker => faker.Random.Guid())
        .RuleFor(sale => sale.Quantity, 1u)
        .RuleFor(sale => sale.UnitPrice, 1m);
        
    /// <summary>
    /// Generates a valid AddSaleItemCommand command with randomized data.
    /// The generated command will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid AddSaleItemCommand command with randomly generated data.</returns>
    public static AddSaleItemCommand Generate()
    {
        return AddSaleItemCommandFaker.Generate();
    }

    public static AddSaleItemCommand WithSaleId(this AddSaleItemCommand command, Guid id)
    {
        command.SaleId = id;
        return command;
    }

    public static AddSaleItemCommand WithProductId(this AddSaleItemCommand command, Guid id)
    {
        command.ProductId = id;
        return command;
    }
    public static AddSaleItemCommand WithUnitPrice(this AddSaleItemCommand command, decimal unitPrice)
    {
        command.UnitPrice = unitPrice;
        return command;
    }

    public static AddSaleItemCommand WithQuantity(this AddSaleItemCommand command, uint quantity)
    {
        command.Quantity = quantity;
        return command;
    }
}
