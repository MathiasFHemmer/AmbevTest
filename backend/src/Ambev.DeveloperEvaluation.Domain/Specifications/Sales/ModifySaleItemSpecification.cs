using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Specifications.Sales;

public sealed class ModifySaleItemSpecification : ISpecification<Sale>
{
    public bool IsSatisfiedBy(Sale entity)
    {
        return entity.Status == SaleStatus.Pending;
    }
}