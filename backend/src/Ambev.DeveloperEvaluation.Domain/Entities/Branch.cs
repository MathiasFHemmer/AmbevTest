using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Mock classes for interacting with the Sales
/// </summary>
public sealed class Branch : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}