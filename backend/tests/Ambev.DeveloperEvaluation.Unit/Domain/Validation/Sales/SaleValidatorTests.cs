using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.Domain.Validation.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation.Sales;

public class SaleValidatorTest
{
    private readonly SaleValidator _validator;

    public SaleValidatorTest()
    {
        _validator = new SaleValidator();
    }

    [Fact(DisplayName = "Valid sale should pass validation")]
    public void Given_ValidSale_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var sale = SaleTestData.Generate().WithSaleItem(SaleItemTestData.Generate());

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "Sale with empty SaleNumber should fail validation")]
    public void Given_EmptySaleNumber_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData
            .Generate()
            .WithSaleNumber(string.Empty);

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Sale with short SaleNumber should fail validation")]
    public void Given_ShortSaleNumber_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.Generate()
            .WithSaleNumber("A");

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Sale with empty CustomerId should fail validation")]
    public void Given_EmptyCustomerId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData
            .Generate()
            .WithCustomerId(Guid.Empty);

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Sale with empty CustomerName should fail validation")]
    public void Given_EmptyCustomerName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData
            .Generate()
            .WithCustomerName(string.Empty);

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Sale with empty BranchId should fail validation")]
    public void Given_EmptyBranchId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData
            .Generate()
            .WithBranchId(Guid.Empty);

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Sale with empty BranchName should fail validation")]
    public void Given_EmptyBranchName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData
            .Generate()
            .WithBranchName(string.Empty);

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(Sale.BranchName));
    }

    [Fact(DisplayName = "Sale with empty SaleItems for Completed status should fail validation")]
    public void Given_CompletedSaleWithNoItems_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData
            .Generate()
            .WithStatus(SaleStatus.Completed);

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Sale with only cancelled SaleItems for Completed status should fail validation")]
    public void Given_CompletedSaleWithOnlyCancelledItems_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData
            .Generate()
            .WithStatus(SaleItemStatus.Cancelled);
            
        var sale = SaleTestData
            .Generate()
            .WithSaleItem(saleItem);

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Sale with Unknown status should fail validation")]
    public void Given_SaleWithUnknownStatus_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.Generate().WithSaleItem(SaleItemTestData.Generate());
        sale.Status = SaleStatus.Unknown;

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(Sale.Status));
    }
}
