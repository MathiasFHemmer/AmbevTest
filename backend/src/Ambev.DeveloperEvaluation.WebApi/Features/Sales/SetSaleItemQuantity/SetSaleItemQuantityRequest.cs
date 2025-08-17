using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.SetSaleItemQuantity;

public sealed class SetSaleItemQuantityRequest
{
    [BindNever]
    [JsonIgnore]
    public Guid SaleId { get; set; } = Guid.Empty;
    [BindNever]
    [JsonIgnore]
    public Guid ProductId { get; set; } = Guid.Empty;
    public uint NewQuantity { get; set; } = 0;
}