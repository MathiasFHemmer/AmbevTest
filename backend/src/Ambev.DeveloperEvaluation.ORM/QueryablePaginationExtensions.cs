using Ambev.DeveloperEvaluation.ORM.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM;

public static class QueryablePaginationExtensions{
    public static async Task<PaginatedList<T>> PaginateAsync<T>(this IQueryable<T> query, int page, int size, CancellationToken cancellationToken)
    {
        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip(page)
            .Take(size)
            .ToListAsync(cancellationToken);

        return PaginatedList<T>.Create(items, (uint)totalItems, (uint)page, (uint)size);
    }
}