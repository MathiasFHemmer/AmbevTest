using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
/// <summary>
/// Handler for processing CreateSaleHandler requests
/// </summary>
public sealed class AddSaleItemHandler : IRequestHandler<AddSaleItemCommand, AddSaleItemResult>
{

    private readonly ISaleRepository _saleRepository;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    public AddSaleItemHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Handles the AddSaleItemCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<AddSaleItemResult> Handle(AddSaleItemCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.SaleId);
        if (sale is null)
            throw new NotFoundException(command.SaleId, typeof(Sale));

        // TODO:
        // Implement product lookup on the repository

        var saleItem = sale.AddItem(command.ProductId, "Placeholder", command.Quantity, command.UnitPrice);
        await _saleRepository.UpdateAsync(sale, cancellationToken);
 
        return new AddSaleItemResult()
        {
            Id = saleItem.Id,
        };

    }
}
