using Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CompleteSale;

public sealed class CompleteSaleProfile : Profile
{
    public CompleteSaleProfile()
    {
        CreateMap<CompleteSaleRequest, CompleteSaleCommand>();
    }
}