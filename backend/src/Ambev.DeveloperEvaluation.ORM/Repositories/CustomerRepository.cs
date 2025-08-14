using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    public Task<Customer> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}