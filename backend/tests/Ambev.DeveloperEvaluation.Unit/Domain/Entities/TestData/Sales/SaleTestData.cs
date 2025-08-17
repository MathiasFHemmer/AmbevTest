using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.Domain.Policies;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;

public static class SaleTestData
{
    private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
        .RuleFor(sale => sale.Status, SaleStatus.Pending)
        .RuleFor(sale => sale.SaleNumber, faker => faker.Random.AlphaNumeric(10))
        .RuleFor(sale => sale.SaleDate, faker => faker.Date.Past())
        .RuleFor(sale => sale.CustomerId, faker => faker.Random.Guid())
        .RuleFor(sale => sale.CustomerName, faker => faker.Person.FullName)
        .RuleFor(sale => sale.BranchId, faker => faker.Random.Guid())
        .RuleFor(sale => sale.BranchName, faker => faker.Company.CompanyName())
        .RuleFor(sale => sale.TotalAmount, faker => faker.Finance.Amount(1, 10000));

    public static Sale Generate()
    {
        return SaleFaker.Generate();
    }

    public static Sale WithDiscountPolicy(this Sale sale, IDiscountPolicy? discountPolicy)
    {
        sale.DiscountPolicy = discountPolicy;
        return sale;
    }

    public static Sale WithStatus(this Sale sale, SaleStatus status)
    {
        sale.Status = status;
        return sale;
    }

    public static Sale WithSaleItem(this Sale sale, SaleItem item)
    {
        item.SaleId = sale.Id;
        sale._saleItems.Add(item);
        return sale;
    }

    public static Sale WithSaleNumber(this Sale sale, string? saleNumber)
    {
        sale.SaleNumber = saleNumber;
        return sale;
    }

    public static Sale WithCustomerId(this Sale sale, Guid customerId)
    {
        sale.CustomerId = customerId;
        return sale;
    }

    public static Sale WithCustomerName(this Sale sale, string? customerName)
    {
        sale.CustomerName = customerName;
        return sale;
    }

    public static Sale WithBranchName(this Sale sale, string? branchName)
    {
        sale.BranchName = branchName;
        return sale;
    }

    public static Sale WithBranchId(this Sale sale, Guid branchId)
    {
        sale.BranchId = branchId;
        return sale;
    }
}