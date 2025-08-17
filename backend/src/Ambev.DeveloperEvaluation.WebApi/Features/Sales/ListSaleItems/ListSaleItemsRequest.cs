namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSaleItems;

public sealed class ListSaleItemsRequest
{
    public Guid SaleId { get; set; } = Guid.Empty;
}