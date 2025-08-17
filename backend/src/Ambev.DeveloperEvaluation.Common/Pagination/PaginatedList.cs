using Serilog;

namespace Ambev.DeveloperEvaluation.ORM.Pagination;

public class PaginatedList<TData>
{
    public IEnumerable<TData> Items { get; set; }
    public uint CurrentPage { get; set; }
    public uint TotalPages { get; set; }
    public uint PageSize { get; set; }
    public uint TotalCount { get; set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    
    public PaginatedList() : this([], 0, 1, 10){}
    public PaginatedList(IEnumerable<TData> items, uint count, uint pageNumber, uint pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = Convert.ToUInt32(Math.Ceiling(count / (double)pageSize));
        Items = items;
    }
}