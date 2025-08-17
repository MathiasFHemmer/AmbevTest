using Ambev.DeveloperEvaluation.Application.Sales.SetSaleItemQuantity;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

/// <summary>
/// Provides methods for generating test data for <see cref="SetSaleItemQuantityCommand"/>
/// using the Bogus library.
/// </summary>
public static class SetSaleItemQuantityHandlerTestData
{
    /// <summary>
    /// Faker configuration to generate valid SetSaleItemQuantityCommand instances
    /// </summary>
    private static readonly Faker<SetSaleItemQuantityCommand> CommandFaker = new Faker<SetSaleItemQuantityCommand>()
        .RuleFor(cmd => cmd.SaleId, f => f.Random.Guid())
        .RuleFor(cmd => cmd.ProductId, f => f.Random.Guid())
        .RuleFor(cmd => cmd.NewQuantity, f => f.Random.UInt(1, 100));

    /// <summary>
    /// Generates a valid SetSaleItemQuantityCommand with randomized data
    /// </summary>
    public static SetSaleItemQuantityCommand Generate()
    {
        return CommandFaker.Generate();
    }

    /// <summary>
    /// Overrides the SaleId property with a specific value
    /// </summary>
    public static SetSaleItemQuantityCommand WithSaleId(this SetSaleItemQuantityCommand command, Guid id)
    {
        command.SaleId = id;
        return command;
    }

    /// <summary>
    /// Overrides the ProductId property with a specific value
    /// </summary>
    public static SetSaleItemQuantityCommand WithProductId(this SetSaleItemQuantityCommand command, Guid id)
    {
        command.ProductId = id;
        return command;
    }

    /// <summary>
    /// Overrides the NewQuantity property with a specific value
    /// </summary>
    public static SetSaleItemQuantityCommand WithNewQuantity(this SetSaleItemQuantityCommand command, uint quantity)
    {
        command.NewQuantity = quantity;
        return command;
    }
}
