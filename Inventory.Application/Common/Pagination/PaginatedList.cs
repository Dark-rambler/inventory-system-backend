namespace Inventory.Application.Common.Pagination
{
    public class PaginatedList<T>(
        List<T> items,
        int totalCount,
        int pageIndex,
        int pageSize)
    {
        public List<T> Items { get; } = items;
        public int PageIndex { get; } = pageIndex;
        public int PageSize { get; } = pageSize;
        public int TotalCount { get; } = totalCount;

        public int TotalPages =>
            (int)Math.Ceiling(TotalCount / (double)PageSize);

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
