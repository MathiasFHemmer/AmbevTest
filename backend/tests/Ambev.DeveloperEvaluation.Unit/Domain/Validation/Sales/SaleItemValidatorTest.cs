using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Validation.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation.Sales;

public class SaleItemValidatorTest
{
    private readonly SaleItemValidator _validator;

    public SaleItemValidatorTest()
    {
        _validator = new SaleItemValidator();
    }

    /// <summary>
    /// Tests that validation passes for a valid sale item.
    /// </summary>
    [Fact(DisplayName = "Valid sale item should pass validation")]
    public void Given_ValidSaleItem_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var saleItem = SaleItemTestData.Generate();

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when the discount is below the lower bound.
    /// </summary>
    [Fact(DisplayName = "Sale item with discount below lower bound should fail validation")]
    public void Given_SaleItemWithInvalidLowerBoundDiscount_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var discount = SaleItemTestData.GenerateInvalidLowerBoundDiscount();
        var saleItem = SaleItemTestData
            .Generate()
            .WithDiscount(discount);

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when the discount is above the upper bound.
    /// </summary>
    [Fact(DisplayName = "Sale item with discount above upper bound should fail validation")]
    public void Given_SaleItemWithInvalidUpperBoundDiscount_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var discount = SaleItemTestData.GenerateInvalidUpperBoundDiscount();
        var saleItem = SaleItemTestData
            .Generate()
            .WithDiscount(discount);

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }


    [Fact(DisplayName = "Sale item with invalid ProductId should fail validation")]
    public void Given_SaleItemWithEmptyProductId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData
            .Generate()
            .WithProductId(Guid.Empty);

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Theory(DisplayName = "Sale item with invalid ProductName should fail validation")]
    [InlineData("")]
    [InlineData(null)]
    public void Given_SaleItemWithEmptyOrNullProductName_When_Validated_Then_ShouldHaveError(string? productName)
    {
        // Arrange
        var saleItem = SaleItemTestData
            .Generate()
            .WithProductName(productName);

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Theory(DisplayName = "Sale item with invalid Quantity should fail validation")]
    [InlineData(0)]
    [InlineData(Sale.MaxItemsPerSale + 1)]
    public void Given_SaleItemWithInvalidQuantity_When_Validated_Then_ShouldHaveError(uint quantity)
    {
        // Arrange
        var saleItem = SaleItemTestData
            .Generate()
            .WithQuantity(quantity);

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact(DisplayName = "Sale item with invalid UnitPrice should fail validation")]
    public void Given_SaleItemWithZeroUnitPrice_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData
            .Generate()
            .WithUnitPrice(0m);
        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}
