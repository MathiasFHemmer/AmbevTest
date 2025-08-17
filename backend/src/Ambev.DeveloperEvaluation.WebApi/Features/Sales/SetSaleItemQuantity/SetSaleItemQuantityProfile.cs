using Ambev.DeveloperEvaluation.Application.Sales.SetSaleItemQuantity;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.SetSaleItemQuantity;

public sealed class SetSaleItemQuantityProfile : Profile
{
    public SetSaleItemQuantityProfile()
    {
        CreateMap<SetSaleItemQuantityRequest, SetSaleItemQuantityCommand>();
    }
}