using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Common.Pagination;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Profile for mapping between Application and API CreateUser responses
/// </summary>
public class ListSalesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser feature
    /// </summary>
    public ListSalesProfile()
    {
        CreateMap<PaginateRequest, ListSaleCommand>()
            .ConstructUsing(x => new ListSaleCommand(x));
        CreateMap<ListSaleResultEntry, ListSaleResponseEntry>();
        CreateMap<ListSaleResult, ListSaleResponse>();
    }
}