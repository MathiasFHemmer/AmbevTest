using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.ORM.Pagination;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSaleItems;

public sealed class ListSaleItemResult : PaginatedList<ListSaleItemResultEntry>
{
    public ListSaleItemResult() : base(Array.Empty<ListSaleItemResultEntry>(), 0, 1, 10) { }

    public ListSaleItemResult(IEnumerable<ListSaleItemResultEntry> items, uint count, uint pageNumber, uint pageSize)
        : base(items, count, pageNumber, pageSize) { }
}

public sealed class ListSaleItemResultEntry
{
    public Guid SaleId { get; set; } = Guid.Empty;
    public Guid ProductId { get; set; } = Guid.Empty;
    public string ProductName { get; set; } = string.Empty;
    public uint Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public SaleItemStatus Status { get; set; } = SaleItemStatus.Unknown;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
