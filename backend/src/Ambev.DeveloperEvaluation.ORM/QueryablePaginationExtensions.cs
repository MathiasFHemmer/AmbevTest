using Ambev.DeveloperEvaluation.ORM.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM;

public static class QueryablePaginationExtensions{
    public static async Task<PaginatedList<T>> PaginateAsync<T>(this IQueryable<T> query, int page, int size, CancellationToken cancellationToken)
    {
        var totalItems = await query.CountAsync(cancellationToken);
        var skip = (page - 1) * size; 
        var items = await query
            .Skip(skip)
            .Take(size)
            .ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, (uint)totalItems, (uint)page, (uint)size);
    }
}