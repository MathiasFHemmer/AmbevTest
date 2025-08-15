namespace Ambev.DeveloperEvaluation.Domain.Exceptions
{
    public class DuplicateItemInSaleException : Exception
    {
        public DuplicateItemInSaleException(string productName) : base($"Product {productName} is already present!")
        {
        }

        public DuplicateItemInSaleException(string productName, Exception innerException) : base($"Product {productName} is already present!", innerException)
        {
        }
    }
}