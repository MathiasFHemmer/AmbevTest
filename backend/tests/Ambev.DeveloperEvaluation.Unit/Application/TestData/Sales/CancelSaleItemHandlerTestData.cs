using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Sales;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CancelSaleItemHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CancelSaleItem command.
    /// The generated command will have valid:
    /// - SaleId (Valid random Guid)
    /// - ProductId (Valid random Guid)
    /// </summary>
    private static readonly Faker<CancelSaleItemCommand> CancelSaleItemCommandFaker = new Faker<CancelSaleItemCommand>()
        .RuleFor(sale => sale.SaleId, faker => faker.Random.Guid())
        .RuleFor(sale => sale.ProductId, faker => faker.Random.Guid());
    /// <summary>
    /// Generates a valid CancelSaleItemCommand command with randomized data.
    /// The generated command will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid CancelSaleItemCommand command with randomly generated data.</returns>
    public static CancelSaleItemCommand Generate()
    {
        return CancelSaleItemCommandFaker.Generate();
    }

    public static CancelSaleItemCommand WithSaleId(this CancelSaleItemCommand command, Guid id)
    {
        command.SaleId = id;
        return command;
    }

    public static CancelSaleItemCommand WithProductId(this CancelSaleItemCommand command, Guid id)
    {
        command.ProductId = id;
        return command;
    }
}
