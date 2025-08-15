using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Command for cancelling a sale item.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for cancelling a sale item, 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="CancelSaleItemResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="CancelSaleItemCommandValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>public sealed class AddSaleItemCommand

public sealed class CancelSaleItemCommand :IRequest<CancelSaleItemResult>
{
    /// <summary>
    /// Gets or Sets the sale identifier
    /// </summary>
    public Guid SaleId { get; set; } = Guid.Empty;
/// <summary>
    /// Gets or Sets the product identifier
    /// </summary>
    public Guid ProductId { get; set; } = Guid.Empty;
}