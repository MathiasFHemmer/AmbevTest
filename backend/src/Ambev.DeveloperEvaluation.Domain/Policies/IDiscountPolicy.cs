using Ambev.DeveloperEvaluation.Domain.Entities.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Policies;

public interface IDiscountPolicy
{
    public decimal GetDiscount(SaleItem item);
}
