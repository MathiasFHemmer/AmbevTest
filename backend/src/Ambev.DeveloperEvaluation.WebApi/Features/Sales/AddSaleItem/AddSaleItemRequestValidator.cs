using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem;

public class AddSaleItemRequestValidator : AbstractValidator<AddSaleItemRequest>
{
    public AddSaleItemRequestValidator()
    {
        RuleFor(sale => sale.ProductId)
            .NotEmpty().WithMessage("ProductId must be greater than zero.");

        RuleFor(sale => sale.Quantity)
            .GreaterThan(0u).WithMessage("Quantity must be greater than zero.");

        RuleFor(sale => sale.UnitPrice)
            .GreaterThan(0m).WithMessage("UnitPrice must be greater than zero.");
    }
}