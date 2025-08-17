using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.ORM.Pagination;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public sealed class ListSaleResult : PaginatedList<ListSaleResultEntry>
{
    public ListSaleResult() : base([], 0, 1, 10){}
    public ListSaleResult(IEnumerable<ListSaleResultEntry> items, uint count, uint pageNumber, uint pageSize) : base(items, count, pageNumber, pageSize) { }
}
public sealed class ListSaleResultEntry
{
    public Guid Id { get; set; } = Guid.Empty;
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

