namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem;

/// <summary>
/// Represents a request to add a sale item to an existing sale.
/// </summary>
public sealed class AddSaleItemRequest
{
    /// <summary>
    /// Gets or Sets the Product Id for the sale item.
    /// </summary>
    public Guid ProductId { get; set; } = Guid.Empty;

    /// <summary>
    /// Gets or Sets the quantity of the product.
    /// </summary>
    public uint Quantity { get; set; } = 0u;

    /// <summary>
    /// Gets or Sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; } = 0m;
}