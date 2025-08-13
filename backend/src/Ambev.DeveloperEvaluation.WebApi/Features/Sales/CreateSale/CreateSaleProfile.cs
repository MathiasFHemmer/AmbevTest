using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

/// <summary>
/// Profile for mapping between Application and API CreateUser responses
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser feature
    /// </summary>
    public CreateSaleProfile()
    {
        // TODO: 
        // 1. Add CreateSaleCommand mappings
        // 2. Add CreateSaleResult mappings
        //CreateMap<CreateSaleRequest, CreateSaleCommand>();
        //CreateMap<CreateUserResult, CreateSaleResponse>();
    }
}
