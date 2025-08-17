using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Sales.SetSaleItemQuantity;

/// <summary>
/// Handler for processing <see cref="SetSaleItemQuantityCommand"/> requests
/// </summary>
public sealed class SetSaleItemQuantityHandler : IRequestHandler<SetSaleItemQuantityCommand, SetSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="SetSaleItemQuantityHandler"/>
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    public SetSaleItemQuantityHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Handles the <see cref="SetSaleItemQuantityCommand"/> request
    /// </summary>
    /// <param name="command">The SetSaleItemQuantity command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="SetSaleItemResult"/> indicating success</returns>
    public async Task<SetSaleItemResult> Handle(SetSaleItemQuantityCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale is null)
            throw new DomainException($"Sale with ID {command.SaleId} not found");

        sale.UpdateItemQuantity(command.ProductId, command.NewQuantity);

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return new SetSaleItemResult();
    }
}
