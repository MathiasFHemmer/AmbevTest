using Ambev.DeveloperEvaluation.Domain.Enums.Sales;

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
    /// <summary>
    /// Gets or Sets this Sale number.
    /// Must not be null or empty.
    /// </summary>
    public string SaleNumber { get; internal set; } = string.Empty;
    /// <summary>
    /// Gets or Sets this Sale customer identifier.
    /// Must not be null or empty.
    /// </summary>
    public Guid CustomerId { get; internal set; } = Guid.Empty;
    /// <summary>
    /// Gets or Sets this Sale the customer name.
    /// Must not be null or empty.
    /// </summary>
    public string CustomerName { get; internal set; } = string.Empty;

    /// <summary>
    ///  Gets or Sets this Sale the branch identifier.
    /// Must not be null or empty.
    /// </summary>
    public Guid BranchId { get; internal set; } = Guid.Empty;
    /// <summary>
    /// Gets or Sets this Sale the branch name.
    /// Must not be null or empty.
    /// </summary>
    public string BranchName { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets or Sets a value indicating the sale status   
    /// </summary>
    public SaleStatus Status { get; internal set; } = SaleStatus.Unknown;
}
