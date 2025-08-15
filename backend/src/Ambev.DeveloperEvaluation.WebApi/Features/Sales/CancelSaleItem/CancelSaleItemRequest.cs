namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;
/// <summary>
/// Represents a request to cancel a sale item on a sale.
/// </summary>
public sealed class CancelSaleItemRequest
{
    /// <summary>
    /// Gets or Sets the sale identifier.
    /// </summary>
    public Guid SaleId { get; set; } = Guid.Empty;
    
    /// <summary>
    /// Gets or Sets the target product identifier for the sale item.
    /// </summary>
    public Guid ProductId { get; set; } = Guid.Empty;

}