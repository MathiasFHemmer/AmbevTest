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
    /// Configures the Faker to generate valid Sale entities.
    /// The generated sale will have valid:
    /// - SaleNumber (valid alphanumeric combination)
    /// - CustomerId (Valid random Guid)
    /// - BranchId (Valid random Guid)
    /// </summary>
    private static readonly Faker<AddSaleItemCommand> AddSaleItemCommandFaker = new Faker<AddSaleItemCommand>()
        .RuleFor(sale => sale.SaleId, faker => faker.Random.Guid())
        .RuleFor(sale => sale.ProductId, faker => faker.Random.Guid())
        .RuleFor(sale => sale.Quantity, 1u)
        .RuleFor(sale => sale.UnitPrice, 1m);
    /// <summary>
    /// Generates a valid AddSaleItemCommand command with randomized data.
    /// The generated sale will have all properties populated with valid values
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
}
