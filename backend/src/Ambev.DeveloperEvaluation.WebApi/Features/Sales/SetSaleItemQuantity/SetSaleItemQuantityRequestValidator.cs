using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.SetSaleItemQuantity;

public sealed class SetSaleItemQuantityRequestValidator : AbstractValidator<SetSaleItemQuantityRequest>
{
    public SetSaleItemQuantityRequestValidator()
    {
        RuleFor(command => command.SaleId)
            .NotEmpty().WithMessage("Sale ID is required");

        RuleFor(command => command.ProductId)
            .NotEmpty().WithMessage("Product ID is required");

        RuleFor(command => command.NewQuantity)
            .GreaterThan(0u).WithMessage("New quantity must be greater than zero")
            .LessThanOrEqualTo(Sale.MaxItemsPerSale).WithMessage($"New quantity must be greater less then or equal to {Sale.MaxItemsPerSale}");
    }
}