namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem
{
    /// <summary>
    /// API response model for AddSaleItem operation
    /// </summary>
    public class AddSaleItemResponse
    {
        /// <summary>
        /// Gets or the newly added Sale Item Identifier
        /// </summary>
        public Guid SaleItemId { get; init; } = Guid.Empty;
    }
}