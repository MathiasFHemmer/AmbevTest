using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Bogus;
using Microsoft.AspNetCore.Builder;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;

public static class SaleItemTestData
{
    private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
        .RuleFor(item => item.ProductId, faker => faker.Random.Guid())
        .RuleFor(item => item.ProductName, faker => faker.Commerce.ProductName())
        .RuleFor(item => item.Quantity, faker => faker.Random.UInt(1, 20))
        .RuleFor(item => item.UnitPrice, faker => faker.Finance.Amount(0.01m, 1000.00m))
        .RuleFor(item => item.Discount, faker => faker.Finance.Amount(0.00m, 1.00m))
        .RuleFor(item => item.Status, SaleItemStatus.Confirmed);

    public static SaleItem Generate()
    {
        return SaleItemFaker.Generate();
    }
    public static SaleItem WithProductId(this SaleItem saleItem, Guid guid)
    {
        saleItem.ProductId = guid;
        return saleItem;
    }

    public static SaleItem WithDiscount(this SaleItem saleItem, decimal discount)
    {
        saleItem.Discount = discount;
        return saleItem;
    }

    public static SaleItem WithProductName(this SaleItem saleItem, string? productName)
    {
        saleItem.ProductName = productName!;
        return saleItem;
    }

    public static SaleItem WithQuantity(this SaleItem saleItem, uint quantity)
    {
        saleItem.Quantity = quantity;
        return saleItem;
    }

    public static SaleItem WithUnitPrice(this SaleItem saleItem, decimal unitPrice)
    {
        saleItem.UnitPrice = unitPrice;
        return saleItem;
    }

    public static SaleItem WithStatus(this SaleItem saleItem, SaleItemStatus status)
    {
        saleItem.Status = status;
        return saleItem;
    }

    public static decimal GenerateInvalidLowerBoundDiscount()
    {
        return new Faker().Random.Decimal(-100.00m, 0);
    }

    public static decimal GenerateInvalidUpperBoundDiscount()
    {
        return new Faker().Random.Decimal(1.01m, 100.00m);
    }
}