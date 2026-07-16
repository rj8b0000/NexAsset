using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class OrganizationRepository : IOrganizationRepository
{
    private readonly ApplicationDbContext _context;

    public OrganizationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByCodeAsync(
        string code,
        CancellationToken cancellationToken)
    {
        return await _context.Organizations
            .AnyAsync(
                x => x.Code == code && !x.IsDeleted,
                cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(
        string code,
        Guid excludeId,
        CancellationToken cancellationToken)
    {
        return await _context.Organizations
            .AnyAsync(
                x => x.Code == code && x.Id != excludeId && !x.IsDeleted,
                cancellationToken);
    }

    public async Task AddAsync(
        Organization organization,
        CancellationToken cancellationToken)
    {
        await _context.Organizations.AddAsync(
            organization,
            cancellationToken);
    }

    public async Task<Organization?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Organizations
            .FirstOrDefaultAsync(
                x => x.Id == id && !x.IsDeleted,
                cancellationToken);
    }

    public async Task<List<Organization>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Organizations
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResponse<Organization>> GetPagedAsync(
        PagedRequest query,
        CancellationToken cancellationToken)
    {
        IQueryable<Organization> queryable =
            _context.Organizations
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            queryable = queryable.Where(x =>
                x.Code.ToLower().Contains(search) ||
                x.Name.ToLower().Contains(search) ||
                x.Email.ToLower().Contains(search));
        }

        queryable = query.SortBy?.ToLower() switch
        {
            "code" => query.Descending
                ? queryable.OrderByDescending(x => x.Code)
                : queryable.OrderBy(x => x.Code),
            "createdat" or "createdatutc" => query.Descending
                ? queryable.OrderByDescending(x => x.CreatedAtUtc)
                : queryable.OrderBy(x => x.CreatedAtUtc),
            _ => query.Descending
                ? queryable.OrderByDescending(x => x.Name)
                : queryable.OrderBy(x => x.Name)
        };

        var totalCount = await queryable.CountAsync(cancellationToken);

        var items = await queryable
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResponse<Organization>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }

    public void Update(Organization organization)
    {
        _context.Organizations.Update(organization);
    }

    public void Delete(Organization organization)
    {
        _context.Organizations.Remove(organization);
    }

    public async Task CascadeSoftDeleteAsync(
        Guid organizationId,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        // Children keyed through Asset / InventoryItem first (subqueries reference the parent by
        // OrganizationId, not IsDeleted, so ordering is safe either way).
        await _context.AssetAssignments
            .Where(x => !x.IsDeleted && _context.Assets.Any(a => a.Id == x.AssetId && a.OrganizationId == organizationId))
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.AssetTransfers
            .Where(x => !x.IsDeleted && _context.Assets.Any(a => a.Id == x.AssetId && a.OrganizationId == organizationId))
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.AssetReturns
            .Where(x => !x.IsDeleted && _context.Assets.Any(a => a.Id == x.AssetId && a.OrganizationId == organizationId))
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.MaintenanceRecords
            .Where(x => !x.IsDeleted && _context.Assets.Any(a => a.Id == x.AssetId && a.OrganizationId == organizationId))
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.StockMovements
            .Where(x => !x.IsDeleted && _context.InventoryItems.Any(i => i.Id == x.InventoryItemId && i.OrganizationId == organizationId))
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.Consumables
            .Where(x => !x.IsDeleted && _context.InventoryItems.Any(i => i.Id == x.InventoryItemId && i.OrganizationId == organizationId))
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);

        // Directly organization-scoped entities.
        await _context.Branches.Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.Departments.Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.Designations.Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.Employees.Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.AssetCategories.Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.Assets.Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.Vendors.Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.PurchaseRequests.Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.PurchaseOrders.Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.InventoryItems.Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.Customers.Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.ServiceTickets.Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
        await _context.SystemSettings.Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true).SetProperty(x => x.DeletedAtUtc, now), cancellationToken);
    }
}