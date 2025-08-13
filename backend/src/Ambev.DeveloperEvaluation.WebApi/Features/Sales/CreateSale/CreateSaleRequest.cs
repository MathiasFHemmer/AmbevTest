namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Sales.CreateSale;

/// <summary>
/// Represents a request to create a new sale in the system.
/// </summary>
public sealed class CreateSaleRequest
{
    /// <summary>
    /// Gets or Sets the Sale Number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    /// <summary>
    /// Gets or Sets the Customer Id
    /// </summary>
    public Guid CustomerId { get; set; } = Guid.Empty;
    
    /// <summary>
    /// Gets or Sets the Branch Id
    /// </summary>
    public Guid BranchId { get; set; } = Guid.Empty;
}