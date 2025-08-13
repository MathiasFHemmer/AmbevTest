using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.Domain.Policies;
using Ambev.DeveloperEvaluation.Domain.Specifications.Sales;
using Ambev.DeveloperEvaluation.Domain.Validation.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sales;

public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets or Sets the product identifier associated with this Sale item.
    /// Must not be null or empty.
    /// </summary>
    public Guid ProductId { get; set; } = Guid.Empty;
    /// <summary>
    /// Gets or Sets the product name associated with this Sale item.
    /// Must not be null or empty.  
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets quantity of product on this Sale item.
    /// Must be greater than zero.
    /// </summary>
    public uint Quantity { get; set; } = 1;

    /// <summary>
    /// Gets or Sets unit price of product on this Sale item.
    /// Must be greater than zero.
    /// </summary>
    public decimal UnitPrice { get; set; } = 0m;


    /// <summary>
    /// Gets discount applied to this Sale item. This is represented as a decimal value between 0 and 1 (e.g., 0.15 for a 15% discount).
    /// This field is optional and defaults to zero if no discount is applied.
    /// This field should not be update directly, but through dedicated methods to ensure business rules are enforced.
    /// </summary>
    public decimal Discount { get; set; } = 0m;

    /// <summary>
    /// Gets or Sets a value indicating the status of the Sale item.
    /// </summary>
    public SaleItemStatus Status { get; set; } = SaleItemStatus.Unknown;

    /// <summary>
    /// Gets or Sets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or Sets the date and time of the last update to the user's information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets the total price for this Sale item (Quantity * UnitPrice * (1-Discount)).
    /// For instance, if Quantity is 2, UnitPrice is 50, and Discount is 0.1 (10%), the TotalPrice will be 90 
    /// Given the values we have =  2 * 50 * (1 - 0.1).
    /// Calculating = 100 * 0.9 = 90
    /// </summary>
    public decimal TotalPrice => Quantity * UnitPrice * (1 - Discount);

    /// <summary>
    /// Gets the amount saved due to the discount applied to this Sale item.
    /// TotalPrice without discount - TotalPrice with discount
    /// </summary>
    public decimal DiscountAmount => (Quantity * UnitPrice) - TotalPrice;

    public SaleItem()
    {
        CreatedAt = DateTime.UtcNow;
    }
    internal SaleItem(Guid productId, string productName, uint quantity, decimal unitPrice)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Status = SaleItemStatus.Confirmed;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Performs validation of the Sale Item entity using the <see cref="SaleItemValidator"/> rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    /// <remarks>
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">ProductId is not empty</list>
    /// <list type="bullet">ProductName is not empty</list>
    /// <list type="bullet">Quantity is greater than zero</list>
    /// <list type="bullet">UnitPrice is greater than zero</list> 
    /// <list type="bullet">Discount is between 0 and 1</list>
    /// </remarks>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    public void UpdateItemQuantity(uint quantity)
    {
        if (Status == SaleItemStatus.Cancelled)
            throw new DomainException("Cannot update amount of a cancelled sale item.");

        if (quantity == 0)
            throw new DomainException("Quantity must be greater than zero.");

        Quantity = quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Applies a discount policy to the current item. The discount will be clamped in the range 0 to 1 (meaning 0% to 100%).
    /// If no discount policy is given, the discount will be 0
    /// </summary>
    /// <param name="discountPolicy"></param>

    public void ApplyDiscountPolicy(IDiscountPolicy? discountPolicy)
    {
        var discount = 0m;
        if(discountPolicy is not null)
            discount = discountPolicy.GetDiscount(this);
        Discount = Math.Clamp(discount, 0, 1);
    }

    public void Cancel()
    {
        if (Status == SaleItemStatus.Cancelled)
            return;

        Status = SaleItemStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
}