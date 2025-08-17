using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem;

public class AddSaleItemRequestValidator : AbstractValidator<AddSaleItemRequest>
{
    public AddSaleItemRequestValidator()
    {
        RuleFor(request => request.ProductId)
            .NotEmpty().WithMessage("ProductId must be greater than zero.");

        RuleFor(request => request.Quantity)
            .GreaterThan(0u).WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(Sale.MaxItemsPerSale).WithMessage($"Quantity must be equal or bellow {Sale.MaxItemsPerSale}.");

        RuleFor(request => request.UnitPrice)
            .GreaterThan(0m).WithMessage("UnitPrice must be greater than zero.");
    }
}