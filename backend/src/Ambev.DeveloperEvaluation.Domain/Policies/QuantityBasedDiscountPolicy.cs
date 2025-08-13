using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Policies;

public class QuantityBasedDiscountPolicy : IDiscountPolicy
{
    public decimal GetDiscount(SaleItem item)
    {
        return item.Quantity switch
        {
            > 0 and <= 4 => 0.00m,
            > 4 and <= 9 => 0.10m,
            > 9 and <= 20 => 0.20m,
            _ => throw new DomainException($"There is no discount policy that matches the quantity ({item.Quantity}) provided!"),
        };
    }
}