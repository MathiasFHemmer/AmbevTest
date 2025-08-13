using Ambev.DeveloperEvaluation.Domain.Entities.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Specifications.Sales;

public class SaleItemQuantityLimitSpecification : ISpecification<SaleItem>
{
    public const uint AmountLimit = Sale.MaxItemsPerSale;
    public bool IsSatisfiedBy(uint quantity)
    {
        return quantity <= AmountLimit;
    }

    public bool IsSatisfiedBy(SaleItem entity)
    {
        return entity.Quantity <= AmountLimit;
    }

    public static SaleItemQuantityLimitSpecification Instance { get; set; } = new();
}