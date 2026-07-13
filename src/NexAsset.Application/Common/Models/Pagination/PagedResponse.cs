namespace NexAsset.Application.Common.Models.Paging;

public class PagedResponse<T>
{
    public IReadOnlyCollection<T> Items { get; init; }
        = [];

    public int TotalCount { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int TotalPages =>
        (int)Math.Ceiling(
            TotalCount / (double)PageSize);
    
    public PagedResponse<TResult> Map<TResult>(
        Func<T, TResult> mapper)
    {
        return new PagedResponse<TResult>
        {
            Items = Items.Select(mapper).ToList(),
            TotalCount = TotalCount,
            PageNumber = PageNumber,
            PageSize = PageSize
        };
    }
}