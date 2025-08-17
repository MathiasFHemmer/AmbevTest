namespace Ambev.DeveloperEvaluation.Domain.Exceptions
{
    public class SaleItemQuantityLimitException : DomainException
    {
        public SaleItemQuantityLimitException(uint maxItemsPerSale) : base($"Cannot add more than {maxItemsPerSale} units of a single product to a sale.")
        {
        }

        public SaleItemQuantityLimitException(uint maxItemsPerSale, Exception innerException) : base($"Cannot add more than {maxItemsPerSale} units of a single product to a sale.", innerException)
        {
        }
    }
}