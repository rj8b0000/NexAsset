using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class AssetCategoryRepository : IAssetCategoryRepository
{
    private readonly ApplicationDbContext _context;
    public AssetCategoryRepository(ApplicationDbContext context) => _context = context;

    public Task<bool> ExistsByCodeAsync(Guid organizationId, string code, CancellationToken cancellationToken) =>
        _context.AssetCategories.AnyAsync(x => x.OrganizationId == organizationId && x.Code == code && !x.IsDeleted, cancellationToken);

    public Task<bool> ExistsByCodeAsync(Guid organizationId, string code, Guid excludeId, CancellationToken cancellationToken) =>
        _context.AssetCategories.AnyAsync(x => x.OrganizationId == organizationId && x.Code == code && x.Id != excludeId && !x.IsDeleted, cancellationToken);

    public async Task AddAsync(AssetCategory category, CancellationToken cancellationToken) =>
        await _context.AssetCategories.AddAsync(category, cancellationToken);

    public Task<AssetCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _context.AssetCategories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public async Task<PagedResponse<AssetCategory>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken)
    {
        IQueryable<AssetCategory> queryable = _context.AssetCategories.AsNoTracking().Where(x => !x.IsDeleted);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            queryable = queryable.Where(x => x.Code.ToLower().Contains(search) || x.Name.ToLower().Contains(search));
        }

        queryable = request.SortBy?.ToLower() switch
        {
            "code" => request.Descending ? queryable.OrderByDescending(x => x.Code) : queryable.OrderBy(x => x.Code),
            "createdat" or "createdatutc" => request.Descending ? queryable.OrderByDescending(x => x.CreatedAtUtc) : queryable.OrderBy(x => x.CreatedAtUtc),
            _ => request.Descending ? queryable.OrderByDescending(x => x.Name) : queryable.OrderBy(x => x.Name)
        };

        var totalCount = await queryable.CountAsync(cancellationToken);
        var items = await queryable.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync(cancellationToken);
        return new PagedResponse<AssetCategory> { Items = items, TotalCount = totalCount, PageNumber = request.PageNumber, PageSize = request.PageSize };
    }

    public void Update(AssetCategory category) => _context.AssetCategories.Update(category);
}
