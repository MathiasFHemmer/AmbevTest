namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem;

public class AddSaleItemResponse
{
    public Guid Id { get; init; } = Guid.Empty;
    public string ProductName { get; init; } = string.Empty;
}