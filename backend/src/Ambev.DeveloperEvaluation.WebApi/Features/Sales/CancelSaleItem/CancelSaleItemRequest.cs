namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

public sealed class CancelSaleItemRequest
{
    public Guid SaleId { get; set; } = Guid.Empty;
    public Guid ProductId { get; set; } = Guid.Empty;

}