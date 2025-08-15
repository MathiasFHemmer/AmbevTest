namespace Ambev.DeveloperEvaluation.Domain.Exceptions
{
    public class DuplicateItemInSale : Exception
    {
        public DuplicateItemInSale(string productName) : base($"Product {productName} is already present!")
        {
        }

        public DuplicateItemInSale(string productName, Exception innerException) : base($"Product {productName} is already present!", innerException)
        {
        }
    }
}