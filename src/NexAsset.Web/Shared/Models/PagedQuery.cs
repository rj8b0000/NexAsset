namespace NexAsset.Web.Shared.Models
{
    /// <summary>
    /// The table/query state the UI sends to the backend for a server-driven list:
    /// page, size, free-text search, and sort. Mirrors NexAsset.API's <c>PagedRequest</c>
    /// query parameters (PageNumber, PageSize, Search, SortBy, Descending) so it serializes
    /// straight onto the query string. Emitted by <see cref="Components"/>' DataTable in server mode.
    /// </summary>
    public sealed record PagedQuery
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public string? Search { get; init; }
        public string? SortBy { get; init; }
        public bool Descending { get; init; }
    }
}
