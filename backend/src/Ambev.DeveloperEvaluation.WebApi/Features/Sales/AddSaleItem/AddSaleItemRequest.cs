namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem;

public sealed class AddSaleItemRequest
{
    public Guid ProductId { get; set; } = Guid.Empty;
    public uint Quantity { get; set; } = 0u;
    public decimal UnitPrice { get; set; } = 0m;
}