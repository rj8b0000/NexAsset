using System;
using System.Collections.Generic;

namespace NexAsset.Web.Infrastructure.API
{
    /// <summary>
    /// Frontend mirror of NexAsset.API's <c>PagedResponse&lt;T&gt;</c>. Carries one page of
    /// <typeparamref name="T"/> plus the totals the table footer and pager need. Reused by
    /// every foundation (and future) list endpoint.
    /// </summary>
    public sealed class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
        public int TotalCount { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
