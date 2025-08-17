using AutoMapper;
using Ambev.DeveloperEvaluation.ORM.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSaleItems;

public class ListSaleItemsProfile : Profile
{
    public ListSaleItemsProfile()
    {
        CreateMap<SaleItem, ListSaleItemResultEntry>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
            .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount));

        CreateMap<PaginatedList<SaleItem>, ListSaleItemResult>();
    }
}
