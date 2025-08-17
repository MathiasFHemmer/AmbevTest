using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Sales;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class ListSalesHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid ListSaleCommand command.
    /// The generated sale will have valid:
    /// - Default pagination parameters
    /// </summary>
    private static readonly Faker<ListSaleCommand> ListSaleCommandFaker = new Faker<ListSaleCommand>()
        .RuleFor(sale => sale.Pagination, PaginateRequest.Default);

    /// <summary>
    /// Generates a valid ListSaleCommand object with randomized data.
    /// The generated command will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid ListSaleCommand command with randomly generated data.</returns>
    public static ListSaleCommand Generate()
    {
        return ListSaleCommandFaker.Generate();
    }
}
