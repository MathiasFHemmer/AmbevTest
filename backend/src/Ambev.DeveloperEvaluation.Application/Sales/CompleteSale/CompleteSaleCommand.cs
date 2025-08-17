using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;

/// <summary>
/// Command for completing a sale item.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for completing a sale.
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="CompleteSaleResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="CompleteSaleCommandValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>public sealed class AddSaleItemCommand
public sealed class CompleteSaleCommand : IRequest<CompleteSaleResult>
{
    /// <summary>
    /// Sale identifier to be completed;
    /// </summary>
    public Guid SaleId { get; set; } = Guid.Empty;
}