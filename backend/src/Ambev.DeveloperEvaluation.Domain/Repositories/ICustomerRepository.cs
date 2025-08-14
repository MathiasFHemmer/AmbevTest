using Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Repository interface for Customer entity operations
/// </summary>
public interface ICustomerRepository
{
    public Task<Customer> GetById(Guid id);
}