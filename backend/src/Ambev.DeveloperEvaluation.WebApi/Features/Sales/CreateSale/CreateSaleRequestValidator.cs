using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public sealed class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleNumber: Must not be null or empty
    /// - CustomerId: Must not be null or empty
    /// - BranchId: Must not be null or empty
    /// </remarks>
    public CreateSaleRequestValidator()
    {
        RuleFor(sale => sale.SaleNumber)
           .NotEmpty().WithMessage("Sale number is required.")
           .MinimumLength(3).WithMessage("Sale number must be at last 3 character long.")
           .MaximumLength(20).WithMessage("Sale number cannot be longer than 20 characters.");

        RuleFor(sale => sale.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(sale => sale.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");
    }
}