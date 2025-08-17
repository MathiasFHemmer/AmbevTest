using Ambev.DeveloperEvaluation.Common.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSaleItems;

/// <summary>
/// Command for retrieving paginated sale items.
/// </summary>
public sealed class ListSaleItemsCommand : IRequest<ListSaleItemsResult>
{
    public Guid SaleId { get; set; } = Guid.Empty;
    public PaginateRequest Pagination { get; set; }

    public ListSaleItemsCommand() : this(PaginateRequest.Default) { }

    public ListSaleItemsCommand(PaginateRequest pagination)
    {
        Pagination = pagination ?? PaginateRequest.Default;
    }
}
