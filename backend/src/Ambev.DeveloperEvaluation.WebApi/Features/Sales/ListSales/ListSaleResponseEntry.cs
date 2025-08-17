using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.ORM.Pagination;

public sealed class ListSaleResponse : PaginatedList<ListSaleResponseEntry>
{
}
public sealed class ListSaleResponseEntry
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; internal set; } = DateTime.UtcNow;
    public Guid CustomerId { get; internal set; } = Guid.Empty;
    public string CustomerName { get; internal set; } = string.Empty;
    public Guid BranchId { get; internal set; } = Guid.Empty;
    public string BranchName { get; internal set; } = string.Empty;
    public decimal TotalAmount { get; internal set; } = 0m;
    public SaleStatus Status { get; internal set; } = SaleStatus.Unknown;
    public DateTime CreatedAt { get; internal set; }
    public DateTime? UpdatedAt { get; internal set; }
    public DateTime? CompletedAt { get; internal set; }
}

// public sealed class ListSaleItem
// {
//     public Guid SaleId { get; set; } = Guid.Empty;
//     public Guid ProductId { get; set; } = Guid.Empty;
//     public string ProductName { get; set; } = string.Empty;
//     public uint Quantity { get; set; } = 1;
//     public decimal UnitPrice { get; set; } = 0m;
//     public decimal Discount { get; set; } = 0m;
//     public decimal TotalAmount => Quantity * UnitPrice;
//     public decimal TotalWithDiscount => TotalAmount * (1 - Discount);
//     public SaleItemStatus Status { get; set; } = SaleItemStatus.Unknown;
//     public DateTime CreatedAt { get; set; }
//     public DateTime? UpdatedAt { get; set; }
// }