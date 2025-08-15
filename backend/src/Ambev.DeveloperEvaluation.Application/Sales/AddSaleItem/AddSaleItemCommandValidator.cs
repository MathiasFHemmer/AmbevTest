using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;

public class AddSaleItemCommandValidator : AbstractValidator<AddSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the AddSaleItemCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleId: Must not be null or empty
    /// - ProductId: Must not be null or empty
    /// - Quantity: Must be greater then 0
    /// - UnitPrice: Must be greater then 0
    /// </remarks>
    public AddSaleItemCommandValidator()
    {
        RuleFor(sale => sale.SaleId)
            .NotEmpty().WithMessage($"{nameof(AddSaleItemCommand.SaleId)} must be greater than zero.");

        RuleFor(sale => sale.ProductId)
            .NotEmpty().WithMessage($"{nameof(AddSaleItemCommand.ProductId)} must be greater than zero.");

        RuleFor(sale => sale.Quantity)
            .GreaterThan(0u).WithMessage($"{nameof(AddSaleItemCommand.Quantity)} must be greater than zero.");

        RuleFor(sale => sale.UnitPrice)
            .GreaterThan(0m).WithMessage($"{nameof(AddSaleItemCommand.UnitPrice)} must be greater than zero.");
    }
}