using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.SetSaleItemQuantity;

/// <summary>
/// Validator for <see cref="SetSaleItemQuantityCommand"/>
/// </summary>
public class SetSaleItemQuantityCommandValidator : AbstractValidator<SetSaleItemQuantityCommand>
{
    /// <summary>
    /// Initializes validation rules for <see cref="SetSaleItemQuantityCommand"/>
    /// </summary>
    public SetSaleItemQuantityCommandValidator()
    {
        RuleFor(command => command.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(command => command.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(command => command.NewQuantity)
            .GreaterThan(0u)
            .WithMessage("New quantity must be greater than zero");
    }
}
