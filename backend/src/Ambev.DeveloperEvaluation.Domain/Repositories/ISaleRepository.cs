using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.ORM.Pagination;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for sale entity operations
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Creates a new sale in the repository
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);



    /// <summary>
    /// Deletes a sale from the repository
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits updates done to the Sale
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// /// <returns>True if the sale was deleted, false if not found</returns>
    Task<bool> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Handles paginated requests for listing sale items
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A paginated list of sales </returns>
    Task<PaginatedList<Sale>> ListSales(PaginateRequest pagination, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of sale items from a sale.
    /// </summary>
    /// <param name="saleId"></param>
    /// <param name="pagination"></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<PaginatedList<SaleItem>> ListSaleItemsBySaleId(Guid saleId, PaginateRequest pagination, CancellationToken cancellationToken = default);
}
