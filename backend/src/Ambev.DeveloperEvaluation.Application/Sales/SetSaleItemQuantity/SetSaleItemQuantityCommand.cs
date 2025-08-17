using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.SetSaleItemQuantity;

public sealed class SetSaleItemQuantityCommand : IRequest<SetSaleItemResult>
{
    public Guid SaleId { get; set; } = Guid.Empty;
    public Guid ProductId { get; set; } = Guid.Empty;

    public uint NewQuantity { get; set; } = 0;
}