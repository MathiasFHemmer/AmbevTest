using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    public Task<Customer> GetById(Guid id)
    {
        return Task.FromResult(new Customer
        {
            Id = id,
            Name = "Placeholder Customer"
        });
    }
}