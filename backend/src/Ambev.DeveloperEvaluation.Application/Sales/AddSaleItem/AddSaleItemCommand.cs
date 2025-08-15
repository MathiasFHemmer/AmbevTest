using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;

/// <summary>
/// Command for adding a new sale item to a a.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for creating a sale item, 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="CreateSaleResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="CreateSaleCommandValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>public sealed class AddSaleItemCommand
public sealed class AddSaleItemCommand : IRequest<AddSaleItemResult>
{
    /// <summary>
    /// Gets or Sets the Sale Id to which the item will be added.
    /// </summary>
    public Guid SaleId { get; set; } = Guid.Empty;

    /// <summary>
    /// Gets or Sets the Product Id for the sale item.
    /// </summary>
    public Guid ProductId { get; set; } = Guid.Empty;

    /// <summary>
    /// Gets or Sets the quantity of the product.
    /// </summary>
    public uint Quantity { get; set; } = 0u;

    /// <summary>
    /// Gets or Sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; } = 0m;
}