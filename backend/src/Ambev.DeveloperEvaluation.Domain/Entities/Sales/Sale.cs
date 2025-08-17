using System.Collections.Immutable;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Policies;
using Ambev.DeveloperEvaluation.Domain.Specifications.Sales;
using Ambev.DeveloperEvaluation.Domain.Validation.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sales;

/// <summary>
/// Represents a Sale entity. 
/// </summary>
public class Sale : BaseEntity
{
    public const uint MaxItemsPerSale = 20;
    /// <summary>
    /// Gets the Sale Discount Policy.
    /// Sales can have a null discount policy, meaning no discount should be applied for this sale.
    /// </summary>
    public IDiscountPolicy? DiscountPolicy { get; internal set; }

    /// <summary>
    /// Gets or Sets this Sale number.
    /// Must not be null or empty.
    /// </summary>
    public string SaleNumber { get; internal set; } = string.Empty;
    /// <summary>
    /// Gets or Sets the Sale date.
    /// Must not be null or empty. 
    /// Defaults to the current date and time if not provided.
    /// </summary>
    public DateTime SaleDate { get; internal set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or Sets this Sale customer identifier.
    /// Must not be null or empty.
    /// </summary>
    public Guid CustomerId { get; internal set; } = Guid.Empty;
    /// <summary>
    /// Gets or Sets this Sale the customer name.
    /// Must not be null or empty.
    /// </summary>
    public string CustomerName { get; internal set; } = string.Empty;

    /// <summary>
    ///  Gets or Sets this Sale the branch identifier.
    /// Must not be null or empty.
    /// </summary>
    public Guid BranchId { get; internal set; } = Guid.Empty;
    /// <summary>
    /// Gets or Sets this Sale the branch name.
    /// Must not be null or empty.
    /// </summary>
    public string BranchName { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets or Sets this Sale the total amount.
    /// Must be greater than zero.
    /// </summary>
    public decimal TotalAmount { get; internal set; } = 0m;
    /// <summary>
    /// Gets or Sets a value indicating the sale status   
    /// </summary>
    public SaleStatus Status { get; internal set; } = SaleStatus.Unknown;

    internal List<SaleItem> _saleItems = new();
    /// <summary>
    /// Gets the collection of active items included in this Sale.
    /// </summary>
    public IReadOnlyCollection<SaleItem> SaleItems { get => _saleItems.Where(item => item.Status == SaleItemStatus.Confirmed).ToList(); }

    /// <summary>
    /// Gets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; internal set; }

    /// <summary>
    /// Gets the date and time of the last update to the user's information.
    /// </summary>
    public DateTime? UpdatedAt { get; internal set; }

    /// <summary>
    /// Gets the date and time of when the Sale was marked as completed.
    /// </summary>
    public DateTime? CompletedAt { get; internal set; }

    /// <summary>
    /// Creates a new Sale.
    /// </summary>
    /// <returns><see cref="Sale"/></returns>
    public static Sale Create(string saleNumber, Guid customerId, string customerName, Guid branchId, string branchName)
    {
        return new Sale
        {
            SaleNumber = saleNumber,
            CustomerId = customerId,
            BranchId = branchId,
            CustomerName = customerName,
            BranchName = branchName,
            CreatedAt = DateTime.UtcNow,
            Status = SaleStatus.Pending
        };
    }

    /// <summary>
    /// Performs validation of the Sale entity using the <see cref="SaleValidator"/> rules.
    /// SaleItems inside the Sale entity are also validated using the SaleItemValidator.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    /// <remarks>
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">SaleNumber is not null or empty</list>
    /// <list type="bullet">SaleDate is not null or empty</list>
    /// <list type="bullet">CustomerId is not null or empty</list>
    /// <list type="bullet">CustomerName is not null or empty</list>
    /// <list type="bullet">BranchId is not null or empty</list>
    /// <list type="bullet">BranchName is not null or empty</list>
    /// <list type="bullet">TotalAmount is greater than zero</list>
    /// <list type="bullet">Items is not null or empty when status is Completed. Not validation otherwise</list>
    /// </remarks>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Updates the Sale discount policy, recalculating discounts for associated items.
    /// Throws a DomainException if the sale is cancelled
    /// </summary>
    /// <param name="discountPolicy"></param>
    public void UpdateDiscountPolicy(IDiscountPolicy? discountPolicy)
    {
        DiscountPolicy = discountPolicy;
        foreach (var item in _saleItems)
            item.ApplyDiscountPolicy(discountPolicy);
    }

    /// <summary>
    /// Cancels the Sale.
    /// </summary>
    public bool Cancel()
    {
        if (Status == SaleStatus.Cancelled)
            return false;

        Status = SaleStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }

    /// <summary>
    /// Completes the Sale
    /// </summary>
    public bool Complete()
    {
        if (Status == SaleStatus.Completed)
            return false;

        if (Status == SaleStatus.Cancelled)
            throw new DomainException($"Cannot set a {SaleStatus.Cancelled} Sale as completed!");

        Status = SaleStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
        CompletedAt = DateTime.UtcNow;
        return true;
    }

    /// <summary>
    /// Adds a new item to the Sale. The item will have its price adjusted based on the discount policy chosen, if any.
    /// Throws a <see cref="DomainException"/> if the Sale is cancelled or if the product is already present on the Sale Item collection.
    /// For updating the item quantity use the correct method
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="productName"></param>
    /// <param name="quantity"></param>
    /// <param name="unitPrice"></param>
    /// <exception cref="DomainException"></exception>
    public SaleItem AddItem(Guid productId, string productName, uint quantity, decimal unitPrice)
    {
        var modifySaleItemSpecification = new ModifySaleItemSpecification();
        if (!modifySaleItemSpecification.IsSatisfiedBy(this))
            throw new DomainException($"Cannot add an item to a {Status} sale!");

        if (GetItem(productId) != null)
            throw new DuplicateItemInSaleException(productName);

        var saleItem = new SaleItem(productId, productName, quantity, unitPrice);

        if (DiscountPolicy is not null)
            saleItem.ApplyDiscountPolicy(DiscountPolicy);

        if (!SaleItemQuantityLimitSpecification.Instance.IsSatisfiedBy(saleItem))
            throw new DomainException($"Cannot add more than {MaxItemsPerSale} units of a single product to a sale.");

        _saleItems.Add(saleItem);
        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;
        return saleItem;
    }

    /// <summary>
    /// Gets an item from this sale
    /// </summary>
    /// <param name="productId"></param>
    /// <returns>SaleItem, if present and active on the current sale, Null otherwise</returns>
    public SaleItem? GetItem(Guid productId)
    {
        return _saleItems.FirstOrDefault(item => item.ProductId == productId && item.Status == SaleItemStatus.Confirmed);
    }

    /// <summary>
    /// Updates a single item quantity. The discount gets recalculated based on the current applied policy.
    /// Throws a DomainException if the sale is cancelled, completed, the product is not present on the current Sale, or the item is cancelled.
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="newQuantity"></param>
    /// <exception cref="DomainException"></exception>
    public void UpdateItemQuantity(Guid productId, uint newQuantity)
    {
        var modifySaleItemSpecification = new ModifySaleItemSpecification();
        if (!modifySaleItemSpecification.IsSatisfiedBy(this))
            throw new DomainException($"Cannot update the quantity of an item on a {Status} sale!");

        var item = _saleItems.FirstOrDefault(item => item.ProductId == productId);
        if (item is null)
            throw new DomainException("Item does not exists on this sale!");

        item.UpdateItemQuantity(newQuantity);
        if (DiscountPolicy is not null)
            item.ApplyDiscountPolicy(DiscountPolicy);

        RecalculateTotal();
    }

    public void CancelItem(Guid productId)
    {
        var modifySaleItemSpecification = new ModifySaleItemSpecification();
        if (!modifySaleItemSpecification.IsSatisfiedBy(this))
            throw new DomainException($"Cannot cancel an item on a {Status} sale!");

        var item = GetItem(productId);
        if (item is null)
            throw new NotFoundException(productId, typeof(SaleItem));

        item.Cancel();
        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        TotalAmount = SaleItems.Sum(item => item.TotalPrice);
    }
}