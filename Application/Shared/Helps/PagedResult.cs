
namespace Application.Shared.Helps
{
    public record PagedResult<TItem>
    {
        public List<TItem> Items { get; init; } = new();
        public int TotalCount { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 100;

        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasNextPage => Page < TotalPages;

        public bool HasPreviousPage => Page > 1;
        public static PagedResult<TItem> Empty(int page =1, int pageSize = 100) => new() { Page = page, PageSize = pageSize };
    }

    public record PagedResult<TITem, TTotal> : PagedResult<TITem>
        where TTotal : class, new()
    {
        public TTotal Total { get; init; } = new();
    }
}
