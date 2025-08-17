using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

public class CompleteSaleCommandValidator : AbstractValidator<CancelSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the CompleteSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleId: Must not be null or empty
    /// </remarks>
    public CompleteSaleCommandValidator()
    {
        RuleFor(sale => sale.SaleId)
            .NotEmpty().WithMessage($"{nameof(CancelSaleItemCommand.SaleId)} must be greater than zero.");
    }
}