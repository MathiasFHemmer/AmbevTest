namespace Ambev.DeveloperEvaluation.ORM.Pagination;

public class PaginatedList<TData>
{
    public IEnumerable<TData> Items { get; private set; }
    public uint CurrentPage { get; private set; }
    public uint TotalPages { get; private set; }
    public uint PageSize { get; private set; }
    public uint TotalCount { get; private set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    
    private PaginatedList(IEnumerable<TData> items, uint count, uint pageNumber, uint pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = Convert.ToUInt32(Math.Ceiling(count / (double)pageSize));
        Items = items;
    }

    public static PaginatedList<TData> Create(IEnumerable<TData> items, uint count, uint pageNumber, uint pageSize)
    {
        return new PaginatedList<TData>(items, count, pageNumber, pageSize);
    }
}