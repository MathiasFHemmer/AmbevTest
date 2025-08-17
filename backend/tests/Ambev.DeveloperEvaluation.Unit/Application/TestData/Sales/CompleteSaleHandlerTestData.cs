using Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CompleteSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CompleteSaleCommand commands.
    /// The generated command will have valid:
    /// - SaleId (Valid random Guid)
    /// </summary>
    private static readonly Faker<CompleteSaleCommand> CompleteSaleCommandFaker = new Faker<CompleteSaleCommand>()
        .RuleFor(cmd => cmd.SaleId, faker => faker.Random.Guid());

    /// <summary>
    /// Generates a valid CompleteSaleCommand with randomized data.
    /// The generated command will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid CompleteSaleCommand with randomly generated data.</returns>
    public static CompleteSaleCommand Generate()
    {
        return CompleteSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Allows overriding the SaleId property with a specific value.
    /// Useful for tests where you need deterministic IDs.
    /// </summary>
    /// <param name="command">The command to modify.</param>
    /// <param name="id">The SaleId to set.</param>
    /// <returns>The modified CompleteSaleCommand instance.</returns>
    public static CompleteSaleCommand WithSaleId(this CompleteSaleCommand command, Guid id)
    {
        command.SaleId = id;
        return command;
    }
}
