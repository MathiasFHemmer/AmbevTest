namespace Ambev.DeveloperEvaluation.Common.Pagination;

public sealed class PaginateRequest
{
    public uint Page { get; set; } = 1u;
    public uint PageSize { get; set; } = 10u;

    public static PaginateRequest Default = new PaginateRequest();
}