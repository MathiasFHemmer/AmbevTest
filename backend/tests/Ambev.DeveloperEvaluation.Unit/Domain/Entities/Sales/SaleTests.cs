using Xunit;
using NSubstitute;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.Domain.Policies;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.Sales;

public class SaleTests
{
    [Fact(DisplayName = "AddItem should throw when sale is cancelled")]
    public void Given_CancelledSale_When_AddingItem_Then_ShouldThrow()
    {
        // Arrange
        var sale = SaleTestData.Generate();
        sale.Cancel();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            sale.AddItem(Guid.NewGuid(), "Product", 1, 10m));
    }

    [Fact(DisplayName = "AddItem should throw when product already exists in sale")]
    public void Given_ExistingProduct_When_AddingSameProduct_Then_ShouldThrow()
    {
        // Arrange
        var saleItem = SaleItemTestData.Generate();
        var sale = SaleTestData
            .Generate()
            .WithSaleItem(saleItem);

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.AddItem(saleItem.ProductId, "Product", 1, 10m));
    }

    [Fact(DisplayName = "AddItem should throw when quantity exceeds maximum allowed per product")]
    public void Given_QuantityAboveLimit_When_AddingItem_Then_ShouldThrow()
    {
        // Arrange
        var sale = SaleTestData.Generate();
        var productId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            sale.AddItem(productId, "Product", Sale.MaxItemsPerSale + 1, 10m));
    }

    [Fact(DisplayName = "AddItem should apply discount policy to new sale item")]
    public void Given_ValidPolicy_When_AddingItem_Then_ShouldApplyDiscount()
    {
        // Arrange
        var discountPolicyMock = Substitute.For<IDiscountPolicy>();
        var expectedDiscount = 0.1m;
        discountPolicyMock.GetDiscount(Arg.Any<SaleItem>()).Returns(expectedDiscount);

        var sale = SaleTestData
            .Generate()
            .WithDiscountPolicy(discountPolicyMock);
        var productId = Guid.NewGuid();

        // Act
        sale.AddItem(productId, "Product", 5, 100m);

        // Assert
        var item = sale.GetItem(productId);
        Assert.NotNull(item);
        Assert.Equal(expectedDiscount, item.Discount);
    }

    [Fact(DisplayName = "AddItem should throw when sale is completed")]
    public void Given_CompletedSale_When_AddingItem_Then_ShouldThrow()
    {
        // Arrange
        var sale = SaleTestData.Generate().WithStatus(SaleStatus.Completed);

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.AddItem(Guid.NewGuid(), "Product", 1, 10m));
    }

    [Fact(DisplayName = "UpdateItemQuantity should throw when sale is cancelled")]
    public void Given_CancelledSale_When_UpdatingItemQuantity_Then_ShouldThrow()
    {
        // Arrange
        var saleItem = SaleItemTestData.Generate();
        var sale = SaleTestData
            .Generate()
            .WithSaleItem(saleItem)
            .WithStatus(SaleStatus.Cancelled);

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.UpdateItemQuantity(saleItem.Id, 2));
    }

    [Fact(DisplayName = "UpdateItemQuantity should throw when sale is completed")]
    public void Given_CompletedSale_When_UpdatingItemQuantity_Then_ShouldThrow()
    {
        // Arrange
        var saleItem = SaleItemTestData.Generate();
        var sale = SaleTestData
            .Generate()
            .WithSaleItem(saleItem)
            .WithStatus(SaleStatus.Completed);

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.UpdateItemQuantity(saleItem.ProductId, 5));
    }

    [Fact(DisplayName = "UpdateItemQuantity should throw when item does not exist")]
    public void Given_NonExistingItem_When_UpdatingQuantity_Then_ShouldThrow()
    {
        // Arrange
        var sale = SaleTestData.Generate();

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.UpdateItemQuantity(Guid.NewGuid(), 2));
    }

    [Fact(DisplayName = "UpdateItemQuantity should update and reapply discount")]
    public void Given_ExistingItem_When_UpdatingQuantity_Then_ShouldReapplyDiscount()
    {
        // Arrange
        var expectedQuantity = 5u;
        var discountPolicyMock = Substitute.For<IDiscountPolicy>();
        var expectedDiscount = 0.1m;
        discountPolicyMock.GetDiscount(Arg.Any<SaleItem>()).Returns(expectedDiscount);

        var saleItem = SaleItemTestData
            .Generate()
            .WithQuantity(10)
            .WithUnitPrice(10);

        var sale = SaleTestData
            .Generate()
            .WithSaleItem(saleItem)
            .WithDiscountPolicy(discountPolicyMock);

        // Act1
        sale.UpdateItemQuantity(saleItem.ProductId, expectedQuantity);

        // Assert
        var item = sale.GetItem(saleItem.ProductId);
        Assert.NotNull(item);
        Assert.Equal(expectedDiscount, item.Discount);
        Assert.Equal(expectedQuantity, item.Quantity);
    }

    [Fact(DisplayName = "Cancel should update status and timestamp")]
    public void Given_ActiveSale_When_Cancelling_Then_ShouldUpdateStatusAndTimestamp()
    {
        // Arrange
        var sale = SaleTestData.Generate();

        // Act
        sale.Cancel();

        // Assert
        Assert.Equal(SaleStatus.Cancelled, sale.Status);
        Assert.NotNull(sale.UpdatedAt);
    }

    [Fact(DisplayName = "CancelItem should throw when sale is cancelled")]
    public void Given_CancelledSale_When_CancellingItem_Then_ShouldThrow()
    {
        // Arrange
        var saleItem = SaleItemTestData.Generate();

        var sale = SaleTestData
            .Generate()
            .WithStatus(SaleStatus.Cancelled)
            .WithSaleItem(saleItem);

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.CancelItem(saleItem.ProductId));
    }

    [Fact(DisplayName = "CancelItem should throw when sale is completed")]
    public void Given_CompletedSale_When_CancellingItem_Then_ShouldThrow()
    {
        // Arrange
        var saleItem = SaleItemTestData.Generate();
        var sale = SaleTestData
            .Generate()
            .WithSaleItem(saleItem)
            .WithStatus(SaleStatus.Completed);

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.CancelItem(saleItem.ProductId));
    }

    [Fact(DisplayName = "CancelItem should throw when item does not exist")]
    public void Given_NonExistingItem_When_CancellingItem_Then_ShouldThrow()
    {
        // Arrange
        var sale = SaleTestData.Generate();

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.CancelItem(Guid.NewGuid()));
    }

    [Fact(DisplayName = "CancelItem should mark item as cancelled")]
    public void Given_ExistingItem_When_CancellingItem_Then_ShouldMarkAsCancelled()
    {
        // Arrange
        var saleItem = SaleItemTestData.Generate();
        var sale = SaleTestData
            .Generate()
            .WithSaleItem(saleItem);

        // Act
        sale.CancelItem(saleItem.ProductId);

        // Assert
        var item = sale.GetItem(saleItem.ProductId);
        Assert.NotNull(item);
        Assert.Equal(SaleItemStatus.Cancelled, item.Status);
    }

    [Fact(DisplayName = "Complete should update status and timestamps")]
    public void Given_ActiveSale_When_Completing_Then_ShouldUpdateStatusAndTimestamps()
    {
        // Arrange
        var sale = SaleTestData.Generate();

        // Act
        sale.Complete();

        // Assert
        Assert.Equal(SaleStatus.Completed, sale.Status);
        Assert.NotNull(sale.UpdatedAt);
        Assert.NotNull(sale.CompletedAt);
    }

    [Fact(DisplayName = "Complete should throw when sale is cancelled")]
    public void Given_CancelledSale_When_Completing_Then_ShouldThrow()
    {
        // Arrange
        var sale = SaleTestData.Generate().WithStatus(SaleStatus.Cancelled);

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.Complete());
    }

    [Fact(DisplayName = "UpdateDiscountPolicy should apply policy to all existing items")]
    public void Given_ExistingItems_When_UpdatingDiscountPolicy_Then_ShouldRecalculateDiscounts()
    {
        // Arrange
        var discountPolicyMock = Substitute.For<IDiscountPolicy>();
        discountPolicyMock.GetDiscount(Arg.Any<SaleItem>()).Returns(0.2m);

        var saleItem1 = SaleItemTestData.Generate().WithUnitPrice(100);
        var saleItem2 = SaleItemTestData.Generate().WithUnitPrice(50);
        var sale = SaleTestData
            .Generate()
            .WithSaleItem(saleItem1)
            .WithSaleItem(saleItem2);

        // Act
        sale.UpdateDiscountPolicy(discountPolicyMock);

        // Assert
        Assert.Equal(0.2m, sale.GetItem(saleItem1.ProductId)!.Discount);
        Assert.Equal(0.2m, sale.GetItem(saleItem2.ProductId)!.Discount);
    }
}
