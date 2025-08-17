namespace Ambev.DeveloperEvaluation.Common.Pagination;

public sealed class PaginateRequest
{
    public const uint MAX_PAGE_SIZE = 100;

    public uint Page { get; set; } = 1u;
    public uint PageSize { get; set; } = 10u;

    public PaginateRequest (uint page, uint size)
    {
        Page = Math.Clamp(page, 0, uint.MaxValue);
        PageSize = Math.Clamp(size, 0, MAX_PAGE_SIZE);
    }
    private PaginateRequest() { }
    public static readonly PaginateRequest Default = new PaginateRequest();
}