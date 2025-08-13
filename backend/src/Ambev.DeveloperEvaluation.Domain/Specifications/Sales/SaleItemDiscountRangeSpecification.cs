using Ambev.DeveloperEvaluation.Domain.Entities.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Specifications.Sales;

public class SaleItemDiscountRangeSpecification : ISpecification<SaleItem>
{
    public static uint MinDiscountQuantityRequirement = 4;
    public bool IsSatisfiedBy(SaleItem entity)
    {
        return entity.Quantity > MinDiscountQuantityRequirement;
    }

    public static SaleItemDiscountRangeSpecification Instance = new();
}
