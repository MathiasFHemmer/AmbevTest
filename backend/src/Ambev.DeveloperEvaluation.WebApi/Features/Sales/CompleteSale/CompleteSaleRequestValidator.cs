using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CompleteSale;

public sealed class CompleteSaleRequestValidator : AbstractValidator<CompleteSaleRequest>
{
    public CompleteSaleRequestValidator()
    {
        RuleFor(request => request.SaleId)
            .NotEmpty().WithMessage($"{nameof(CompleteSaleRequest.SaleId)} must not be empty!");
    }
}