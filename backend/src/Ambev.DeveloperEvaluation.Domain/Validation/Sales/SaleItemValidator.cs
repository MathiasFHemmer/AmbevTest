using System.Data;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation.Sales;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty().WithMessage("ProductId must not be empty.")
            .NotEqual(Guid.Empty).WithMessage("ProductId must not be an empty GUID.");

        RuleFor(item => item.ProductName)
            .NotEmpty().WithMessage("ProductName must not be empty.");

        RuleFor(item => item.Quantity)
            .GreaterThan((uint)0).WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(Sale.MaxItemsPerSale).WithMessage($"Quantity must not exceed {Sale.MaxItemsPerSale}.");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0).WithMessage("UnitPrice must be greater than zero.");

        RuleFor(item => item.Discount)
            .InclusiveBetween(0, 1).WithMessage("Discount must be between 0 and 1.");
            
        RuleFor(item => item.Status)
            .NotEqual(SaleItemStatus.Unknown).WithMessage("Sale status cannot be unknown");
    }
}