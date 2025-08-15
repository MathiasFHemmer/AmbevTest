using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

public class CancelSaleItemCommandValidator : AbstractValidator<CancelSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the AddSaleItemCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleId: Must not be null or empty
    /// - SaleItem: Must not be null or empty
    /// </remarks>
    public CancelSaleItemCommandValidator()
    {
        RuleFor(sale => sale.SaleId)
            .NotEmpty().WithMessage($"{nameof(CancelSaleItemCommand.SaleId)} must be greater than zero.");

        RuleFor(sale => sale.ProductId)
            .NotEmpty().WithMessage($"{nameof(CancelSaleItemCommand.ProductId)}");
    }
}