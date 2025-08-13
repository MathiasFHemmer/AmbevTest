using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation.Sales;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required.")
            .MinimumLength(3).WithMessage("Sale number must be at last 3 character long.")
            .MaximumLength(20).WithMessage("Sale number cannot be longer than 20 characters.");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty().WithMessage("Sale date is required.");

        RuleFor(sale => sale.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(sale => sale.CustomerName)
            .NotEmpty().WithMessage("Customer name is required.")
            .MaximumLength(100).WithMessage("Customer name cannot be longer than 100 characters.");

        RuleFor(sale => sale.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");

        RuleFor(sale => sale.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .MaximumLength(100).WithMessage("Branch name cannot be longer than 100 characters.");

        RuleFor(sale => sale.SaleItems)
            .NotEmpty().When(sale => sale.Status == SaleStatus.Completed).WithMessage("At least one Sale item is required.")
            .Must(items => items.Any(item => item.Status == SaleItemStatus.Confirmed)).WithMessage("At least one Sale item is required.");

        RuleFor(sale => sale.Status)
            .NotEqual(SaleStatus.Unknown).WithMessage("Sale cannot be unknown.");

        RuleFor(sale => sale.SaleItems)
            .NotEmpty().WithMessage("At least one product is required for the Sale.");
            
        RuleForEach(sale => sale.SaleItems)
            .SetValidator(new SaleItemValidator());
    }
}