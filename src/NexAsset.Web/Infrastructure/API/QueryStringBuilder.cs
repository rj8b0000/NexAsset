using System;
using System.Collections.Generic;
using System.Linq;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.API
{
    /// <summary>
    /// Turns a <see cref="PagedQuery"/> into the query string NexAsset.API's PagedRequest binds
    /// from (PageNumber, PageSize, Search, SortBy, Descending). Kept in one place so every list
    /// client produces identical, correctly-encoded URLs.
    /// </summary>
    public static class QueryStringBuilder
    {
        public static string ForPagedQuery(string path, PagedQuery query)
        {
            var parts = new List<string>
            {
                $"PageNumber={query.PageNumber}",
                $"PageSize={query.PageSize}",
                $"Descending={query.Descending.ToString().ToLowerInvariant()}"
            };

            if (!string.IsNullOrWhiteSpace(query.Search))
                parts.Add($"Search={Uri.EscapeDataString(query.Search)}");
            if (!string.IsNullOrWhiteSpace(query.SortBy))
                parts.Add($"SortBy={Uri.EscapeDataString(query.SortBy)}");

            return $"{path}?{string.Join("&", parts)}";
        }
    }
}
