using Ambev.DeveloperEvaluation.Common.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Command for retrieving a list of sales.
/// </summary>
/// <remarks>
/// This command is used to capture the querying sales data, 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="ListSaleResult"/>.
/// </remarks>
public sealed class ListSaleCommand : IRequest<ListSaleResult>
{
    public PaginateRequest Pagination { get; set; }

    
    public ListSaleCommand() : this(PaginateRequest.Default){}
    public ListSaleCommand(PaginateRequest pagination)
    {
        Pagination = pagination ?? PaginateRequest.Default;
    }    
}