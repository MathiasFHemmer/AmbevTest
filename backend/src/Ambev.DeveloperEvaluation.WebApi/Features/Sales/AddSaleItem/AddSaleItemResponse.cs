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
        public Guid Id { get; init; } = Guid.Empty;

        /// <summary>
        /// Gets or the newly added Sale Item product name
        /// </summary>
        public Guid ProductName { get; init; } = Guid.Empty;
    }
}