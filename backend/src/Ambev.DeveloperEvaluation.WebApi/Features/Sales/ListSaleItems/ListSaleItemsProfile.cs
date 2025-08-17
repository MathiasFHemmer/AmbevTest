using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Application.Sales.ListSaleItems;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSaleItems;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Profile for mapping between Application and API CreateUser responses
/// </summary>
public class ListSaleItemsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser feature
    /// </summary>
    public ListSaleItemsProfile()
    {
        CreateMap<PaginateRequest, ListSaleItemsCommand>()
            .ConstructUsing(x => new ListSaleItemsCommand(x));
        CreateMap<ListSaleItemResultEntry, ListSaleItemResponseEntry>();
        CreateMap<ListSaleItemsResult, ListSaleItemsResponse>();
    }
}