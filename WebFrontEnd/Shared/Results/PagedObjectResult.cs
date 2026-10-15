namespace WebFrontEnd.Shared.Results
{
    public sealed class PagedObjectResult<T>
    {
        public T? Item { get; init; }

        public int TotalCount { get; init; }

        public int PageNumber { get; init; }

        public int PageSize { get; init; }

        public int TotalPages =>
            PageSize == 0
                ? 0
                : (int)Math.Ceiling(TotalCount / (double)PageSize);

        public bool HasNextPage =>
            PageNumber < TotalPages;
    }
}
