using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSaleItems;
using FluentValidation;

public sealed class ListSaleItemsRequestValidator : AbstractValidator<ListSaleItemsRequest>
{
    public ListSaleItemsRequestValidator()
    {
        RuleFor(request => request.SaleId)
            .NotEmpty().WithMessage($"{nameof(ListSaleItemsRequest.SaleId)} must be a valid id");
    }
}