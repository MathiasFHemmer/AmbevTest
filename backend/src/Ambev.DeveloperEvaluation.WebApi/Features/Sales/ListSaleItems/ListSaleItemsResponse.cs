using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.ORM.Pagination;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSaleItems;


public sealed class ListSaleItemsResponse :  PaginatedList<ListSaleItemResponseEntry>
{
    
}
public sealed class ListSaleItemResponseEntry
{
    public Guid SaleId { get; set; } = Guid.Empty;
    public Guid ProductId { get; set; } = Guid.Empty;
    public string ProductName { get; set; } = string.Empty;
    public uint Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; } = 0m;
    public decimal Discount { get; set; } = 0m;
    public decimal TotalAmount { get => Quantity * UnitPrice; }
    public decimal TotalWithDiscount { get => TotalAmount * (1 - Discount); }
    public SaleItemStatus Status { get; set; } = SaleItemStatus.Unknown;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}