using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;
public class CancelSaleItemRequestValidator : AbstractValidator<CancelSaleItemRequest>
{
    public CancelSaleItemRequestValidator()
    {
        RuleFor(request => request.SaleId)
            .NotEmpty().WithMessage($"{nameof(CancelSaleItemRequest.SaleId)} must be greater than zero.");

        RuleFor(request => request.ProductId)
            .NotEmpty().WithMessage($"{nameof(CancelSaleItemRequest.ProductId)} must be greater than zero.");
    }
}