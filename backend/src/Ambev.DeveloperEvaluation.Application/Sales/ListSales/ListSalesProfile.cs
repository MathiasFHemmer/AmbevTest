using AutoMapper;
using Ambev.DeveloperEvaluation.ORM.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    /// <summary>
    /// Profile for mapping from PaginatedResult<Sale> to ListSaleResult
    /// </summary>
    public class ListSalesProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for ListSaleCommand operation
        /// </summary>
        public ListSalesProfile()
        {
            CreateMap<PaginatedList<Sale>, ListSaleResult>()
                .ForMember(result => result.Data, opt => opt.MapFrom(src => src));
        }
    }
}