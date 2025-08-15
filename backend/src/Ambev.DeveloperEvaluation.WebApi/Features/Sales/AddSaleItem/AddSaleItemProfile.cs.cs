using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

/// <summary>
/// Profile for mapping between Application and API CreateUser responses
/// </summary>
public class AddSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser feature
    /// </summary>
    public AddSaleItemProfile()
    {
        CreateMap<AddSaleItemRequest, AddSaleItemCommand>();
        CreateMap<AddSaleItemResult, AddSaleItemResponse>();
    }
}
