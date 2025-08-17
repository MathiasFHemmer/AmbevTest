namespace Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;
/// <summary>
/// Represents the response returned after successfully creating a new sale item.
/// </summary>
/// <remarks>
/// This response contains the unique identifier of the newly created sale item,
/// which can be used for subsequent operations or reference.
/// </remarks>
public sealed class AddSaleItemResult
{
    /// <summary>
    /// Gets or Sets the created sale Id
    /// </summary>
    public Guid Id { get; set; } = Guid.Empty;
    
    /// <summary>
    /// Gets or the newly added Sale Item product name
    /// </summary>
    public string ProductName { get; set; } = string.Empty;
}