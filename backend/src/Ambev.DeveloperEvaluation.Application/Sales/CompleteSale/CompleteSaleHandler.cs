using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;

public sealed class CompleteSaleHandler : IRequestHandler<CompleteSaleCommand, CompleteSaleResult>
{
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    /// Initializes a new instance of CompleteSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    public CompleteSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Handles the CompleteSaleHandler request
    /// </summary>
    /// <param name="command">The CompleteSaleCommand command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CompleteSaleResult marker object if successful</returns>
    public async Task<CompleteSaleResult> Handle(CompleteSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.SaleId);
        if (sale is null)
            throw new NotFoundException(command.SaleId, typeof(Sale));

        if(sale.Complete())
            await _saleRepository.UpdateAsync(sale, cancellationToken);

        return new CompleteSaleResult();
    }
}