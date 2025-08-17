using Ambev.DeveloperEvaluation.Common.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSaleItems;

/// <summary>
/// Command for retrieving paginated sale items.
/// </summary>
public sealed class ListSaleItemCommand : IRequest<ListSaleItemResult>
{
    public Guid SaleId { get; set; } = Guid.Empty;
    public PaginateRequest Pagination { get; set; }

    public ListSaleItemCommand() : this(PaginateRequest.Default) { }

    public ListSaleItemCommand(PaginateRequest pagination)
    {
        Pagination = pagination ?? PaginateRequest.Default;
    }
}
