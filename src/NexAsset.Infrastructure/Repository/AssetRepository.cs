using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class AssetRepository : IAssetRepository
{
    private readonly ApplicationDbContext _context;
    public AssetRepository(ApplicationDbContext context) => _context = context;

    public Task<bool> ExistsByCodeAsync(Guid organizationId, string assetCode, CancellationToken cancellationToken) =>
        _context.Assets.AnyAsync(x => x.OrganizationId == organizationId && x.AssetCode == assetCode && !x.IsDeleted, cancellationToken);

    public Task<bool> ExistsByCodeAsync(Guid organizationId, string assetCode, Guid excludeId, CancellationToken cancellationToken) =>
        _context.Assets.AnyAsync(x => x.OrganizationId == organizationId && x.AssetCode == assetCode && x.Id != excludeId && !x.IsDeleted, cancellationToken);

    public Task<bool> ExistsBySerialNumberAsync(string serialNumber, CancellationToken cancellationToken) =>
        _context.Assets.AnyAsync(x => x.SerialNumber == serialNumber && !x.IsDeleted, cancellationToken);

    public Task<bool> ExistsBySerialNumberAsync(string serialNumber, Guid excludeId, CancellationToken cancellationToken) =>
        _context.Assets.AnyAsync(x => x.SerialNumber == serialNumber && x.Id != excludeId && !x.IsDeleted, cancellationToken);

    public async Task AddAsync(Asset asset, CancellationToken cancellationToken) =>
        await _context.Assets.AddAsync(asset, cancellationToken);

    public Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _context.Assets.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public async Task<PagedResponse<Asset>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken)
    {
        IQueryable<Asset> queryable = _context.Assets.AsNoTracking().Where(x => !x.IsDeleted);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            queryable = queryable.Where(x =>
                x.AssetCode.ToLower().Contains(search) ||
                x.AssetName.ToLower().Contains(search) ||
                (x.SerialNumber != null && x.SerialNumber.ToLower().Contains(search)) ||
                (x.Barcode != null && x.Barcode.ToLower().Contains(search)) ||
                x.Category.Name.ToLower().Contains(search) ||
                (x.Branch != null && x.Branch.Name.ToLower().Contains(search)) ||
                (x.Department != null && x.Department.Name.ToLower().Contains(search)) ||
                (x.CurrentEmployee != null &&
                 (x.CurrentEmployee.FirstName.ToLower().Contains(search) ||
                  x.CurrentEmployee.LastName.ToLower().Contains(search) ||
                  x.CurrentEmployee.EmployeeCode.ToLower().Contains(search))));
        }

        queryable = request.SortBy?.ToLower() switch
        {
            "assetcode" or "code" => request.Descending ? queryable.OrderByDescending(x => x.AssetCode) : queryable.OrderBy(x => x.AssetCode),
            "assetname" or "name" => request.Descending ? queryable.OrderByDescending(x => x.AssetName) : queryable.OrderBy(x => x.AssetName),
            "purchasedate" => request.Descending ? queryable.OrderByDescending(x => x.PurchaseDate) : queryable.OrderBy(x => x.PurchaseDate),
            "warrantyexpiry" => request.Descending ? queryable.OrderByDescending(x => x.WarrantyExpiry) : queryable.OrderBy(x => x.WarrantyExpiry),
            "createdat" or "createdatutc" => request.Descending ? queryable.OrderByDescending(x => x.CreatedAtUtc) : queryable.OrderBy(x => x.CreatedAtUtc),
            _ => request.Descending ? queryable.OrderByDescending(x => x.AssetName) : queryable.OrderBy(x => x.AssetName)
        };

        var totalCount = await queryable.CountAsync(cancellationToken);
        var items = await queryable.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync(cancellationToken);
        return new PagedResponse<Asset> { Items = items, TotalCount = totalCount, PageNumber = request.PageNumber, PageSize = request.PageSize };
    }

    public void Update(Asset asset) => _context.Assets.Update(asset);
}
