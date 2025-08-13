namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Sales.CreateSale;
/// <summary>
/// API response model for CreateSale operation
/// </summary>
public sealed class CreateSaleResponse
{
    /// <summary>
    /// Gets or Sets the created sale Id
    /// </summary>
    public Guid Id { get; set; } = Guid.Empty;
}