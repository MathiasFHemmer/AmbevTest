using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.ORM.Pagination;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public sealed class ListSaleResult
{
    public required PaginatedList<Sale> Data { get; set; }
}