using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Application.Sales.ListSaleItems;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSaleItems;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class ListSaleItemsProfile : Profile
{
    public ListSaleItemsProfile()
    {
        CreateMap<ListSaleItemsRequest, ListSaleItemsCommand>();
        CreateMap<ListSaleItemResultEntry, ListSaleItemResponseEntry>();
        CreateMap<ListSaleItemsResult, ListSaleItemsResponse>();
    }
}