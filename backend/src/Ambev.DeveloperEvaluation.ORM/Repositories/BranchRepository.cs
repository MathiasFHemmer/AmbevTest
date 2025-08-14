using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public sealed class BranchRepository : IBranchRepository
{
    public Task<Branch> GetById(Guid id)
    {
        return Task.FromResult(new Branch
        {
            Id = id,
            Name = "Placeholder Branch"
        });
    }
}