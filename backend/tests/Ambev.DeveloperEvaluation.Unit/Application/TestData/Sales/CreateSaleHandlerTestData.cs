using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Sales;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateSaleCommand.
    /// The generated command will have valid:
    /// - SaleNumber (valid alphanumeric combination)
    /// - CustomerId (Valid random Guid)
    /// - BranchId (Valid random Guid)
    /// </summary>
    private static readonly Faker<CreateSaleCommand> CreateSaleCommandFaker = new Faker<CreateSaleCommand>()
        .RuleFor(sale => sale.SaleNumber, faker => faker.Random.AlphaNumeric(10))
        .RuleFor(sale => sale.CustomerId, faker => faker.Random.Guid())
        .RuleFor(sale => sale.BranchId, faker => faker.Random.Guid());
    /// <summary>
    /// Generates a valid CreateSaleCommand object with randomized data.
    /// The generated command will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid CreateSaleCommand command with randomly generated data.</returns>
    public static CreateSaleCommand Generate()
    {
        return CreateSaleCommandFaker.Generate();
    }
}
