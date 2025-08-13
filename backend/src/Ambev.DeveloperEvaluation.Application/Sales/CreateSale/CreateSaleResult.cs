namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
/// <summary>
/// Represents the response returned after successfully creating a new sale.
/// </summary>
/// <remarks>
/// This response contains the unique identifier of the newly created sale,
/// which can be used for subsequent operations or reference.
/// </remarks>
public sealed class CreateSaleResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the newly created sale.
    /// </summary>
    public Guid Id { get; set; }
}
