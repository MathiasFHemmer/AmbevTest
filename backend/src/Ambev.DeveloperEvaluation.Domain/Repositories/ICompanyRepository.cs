using Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Repository interface for Company and Branch entity operations
/// </summary>
public interface IBranchRepository
{
    public Task<Branch> GetById(Guid id);
}