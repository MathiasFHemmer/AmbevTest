using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.Domain.Policies;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.Sales;

public class SaleItemTests
{
    [Fact(DisplayName = "Cancel should change status to cancelled when cancelled")]
    public void Given_ConfirmedSaleItem_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        //Arrange
        var saleItem = SaleItemTestData.Generate();

        //Act
        saleItem.Cancel();

        //Assert
        Assert.Equal(SaleItemStatus.Cancelled, saleItem.Status);
    }

    [Fact(DisplayName = "UpdateItemQuantity should change quantity if valid amount is set")]
    public void Given_ValidQuantity_When_UpdatingQuantity_Then_QuantityShouldChange()
    {
        //Arrange
        var saleItem = SaleItemTestData.Generate();
        var newQuantity = 10u;

        //Act
        saleItem.UpdateItemQuantity(newQuantity);

        //Assert
        Assert.Equal(newQuantity, saleItem.Quantity);
    }

    [Fact(DisplayName = "UpdateItemQuantity throw if invalid amount is set")]
    public void Given_InvalidQuantity_When_UpdatingQuantity_Then_ShouldThrowException()
    {
        //Arrange
        var saleItem = SaleItemTestData.Generate();
        var newQuantity = 0u;

        //Act & Assert
        Assert.Throws<DomainException>(() => saleItem.UpdateItemQuantity(newQuantity));
    }

    [Fact(DisplayName = "UpdateItemQuantity should throw if item is cancelled")]
    public void Given_CancelledItem_When_UpdatingQuantity_Then_ShouldThrowException()
    {
        //Arrange
        var saleItem = SaleItemTestData.Generate();
        saleItem.Cancel();

        var newQuantity = 0u;

        //Act & Assert
        Assert.Throws<DomainException>(() => saleItem.UpdateItemQuantity(newQuantity));
    }

    [Fact(DisplayName = "ApplyDiscountPolicy should modify the discount value")]
    public void Given_ValidDiscountPolicy_When_Applying_Then_ShouldChangeDiscountValue()
    {
        //Arrange
        var saleItem = SaleItemTestData.Generate();

        var expectedDiscount = 0.5m;
        var discountPolicyMock = Substitute.For<IDiscountPolicy>();
        discountPolicyMock.GetDiscount(Arg.Any<SaleItem>()).ReturnsForAnyArgs(expectedDiscount);

        //Act
        saleItem.ApplyDiscountPolicy(discountPolicyMock);
        //Assert
        Assert.Equal(expectedDiscount, saleItem.Discount);
    }
    
    [Fact(DisplayName = "ApplyDiscountPolicy should clamp the discount value to 1 if value is exceeded ")]
    public void Given_DiscountPolicy_When_Applying_Then_ShouldClampDiscountValue()
    {
        // Arrange
        var saleItem = SaleItemTestData.Generate();

        var appliedDiscountRate = 100m;
        var expectedDiscountRate = 1m;
        var discountPolicyMock = Substitute.For<IDiscountPolicy>();
        discountPolicyMock.GetDiscount(Arg.Any<SaleItem>()).Returns(appliedDiscountRate);

        // Act
        saleItem.ApplyDiscountPolicy(discountPolicyMock);

        // Assert
        Assert.Equal(expectedDiscountRate, saleItem.Discount);
    }
}