using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public sealed class BranchRepository : IBranchRepository
{
    public Task<Branch> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}