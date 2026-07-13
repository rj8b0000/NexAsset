using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Common;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class EnterpriseOperationsRepository : IEnterpriseOperationsRepository
{
    private readonly ApplicationDbContext _context;

    public EnterpriseOperationsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync<TEntity>(
        Guid organizationId,
        string code,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        var queryable = _context.Set<TEntity>().AsNoTracking();

        return typeof(TEntity).Name switch
        {
            nameof(Vendor) => await queryable.Cast<Vendor>()
                .AnyAsync(x => x.OrganizationId == organizationId && x.Code == code && !x.IsDeleted, cancellationToken),
            nameof(Customer) => await queryable.Cast<Customer>()
                .AnyAsync(x => x.OrganizationId == organizationId && x.Code == code && !x.IsDeleted, cancellationToken),
            nameof(InventoryItem) => await queryable.Cast<InventoryItem>()
                .AnyAsync(x => x.OrganizationId == organizationId && x.ItemCode == code && !x.IsDeleted, cancellationToken),
            _ => false
        };
    }

    public async Task AddAsync<TEntity>(
        TEntity entity,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync<TEntity>(
        Guid id,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        IQueryable<TEntity> queryable = _context.Set<TEntity>();

        if (typeof(BaseEntity).IsAssignableFrom(typeof(TEntity)))
        {
            return await queryable
                .Cast<BaseEntity>()
                .Where(x => x.Id == id && !x.IsDeleted)
                .Cast<TEntity>()
                .FirstOrDefaultAsync(cancellationToken);
        }

        return await queryable.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResponse<TEntity>> GetPagedAsync<TEntity>(
        PagedRequest request,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        IQueryable<TEntity> queryable = _context.Set<TEntity>().AsNoTracking();

        if (typeof(BaseEntity).IsAssignableFrom(typeof(TEntity)))
        {
            queryable = queryable
                .Cast<BaseEntity>()
                .Where(x => !x.IsDeleted)
                .Cast<TEntity>();
        }

        queryable = ApplySearch(queryable, request.Search);
        queryable = ApplySorting(queryable, request.SortBy, request.Descending);

        var totalCount = await queryable.CountAsync(cancellationToken);

        var items = await queryable
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResponse<TEntity>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public void Update<TEntity>(TEntity entity)
        where TEntity : class
    {
        _context.Set<TEntity>().Update(entity);
    }

    public Task<InventoryItem?> GetInventoryItemAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return _context.InventoryItems
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public Task<List<StockMovement>> GetStockHistoryAsync(
        Guid inventoryItemId,
        CancellationToken cancellationToken)
    {
        return _context.StockMovements
            .AsNoTracking()
            .Where(x => x.InventoryItemId == inventoryItemId && !x.IsDeleted)
            .OrderByDescending(x => x.MovementAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<List<PurchaseRequest>> GetPurchaseRequestHistoryAsync(
        Guid organizationId,
        CancellationToken cancellationToken)
    {
        return _context.PurchaseRequests
            .AsNoTracking()
            .Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<List<MaintenanceRecord>> GetMaintenanceHistoryAsync(
        Guid assetId,
        CancellationToken cancellationToken)
    {
        return _context.MaintenanceRecords
            .AsNoTracking()
            .Where(x => x.AssetId == assetId && !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<List<ServiceTicket>> GetTicketHistoryAsync(
        Guid customerId,
        CancellationToken cancellationToken)
    {
        return _context.ServiceTickets
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId && !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<SystemSetting?> GetSystemSettingAsync(
        Guid? organizationId,
        string key,
        CancellationToken cancellationToken)
    {
        return _context.SystemSettings
            .FirstOrDefaultAsync(
                x => x.OrganizationId == organizationId &&
                     x.Key == key &&
                     !x.IsDeleted,
                cancellationToken);
    }

    public Task<int> CountAsync<TEntity>(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> filter,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        return filter(_context.Set<TEntity>().AsNoTracking())
            .CountAsync(cancellationToken);
    }

    private static IQueryable<TEntity> ApplySearch<TEntity>(
        IQueryable<TEntity> queryable,
        string? search)
        where TEntity : class
    {
        if (string.IsNullOrWhiteSpace(search))
            return queryable;

        var value = search.ToLower();

        return typeof(TEntity).Name switch
        {
            nameof(Vendor) => queryable.Cast<Vendor>()
                .Where(x => x.Code.ToLower().Contains(value) || x.Name.ToLower().Contains(value) || x.Email.ToLower().Contains(value))
                .Cast<TEntity>(),
            nameof(Customer) => queryable.Cast<Customer>()
                .Where(x => x.Code.ToLower().Contains(value) || x.Name.ToLower().Contains(value) || x.Email.ToLower().Contains(value))
                .Cast<TEntity>(),
            nameof(PurchaseRequest) => queryable.Cast<PurchaseRequest>()
                .Where(x => x.RequestNumber.ToLower().Contains(value) || x.Title.ToLower().Contains(value))
                .Cast<TEntity>(),
            nameof(PurchaseOrder) => queryable.Cast<PurchaseOrder>()
                .Where(x => x.OrderNumber.ToLower().Contains(value))
                .Cast<TEntity>(),
            nameof(InventoryItem) => queryable.Cast<InventoryItem>()
                .Where(x => x.ItemCode.ToLower().Contains(value) || x.ItemName.ToLower().Contains(value))
                .Cast<TEntity>(),
            nameof(Consumable) => queryable.Cast<Consumable>()
                .Where(x => x.ConsumableCode.ToLower().Contains(value) || x.Name.ToLower().Contains(value))
                .Cast<TEntity>(),
            nameof(MaintenanceRecord) => queryable.Cast<MaintenanceRecord>()
                .Where(x => x.Title.ToLower().Contains(value))
                .Cast<TEntity>(),
            nameof(ServiceTicket) => queryable.Cast<ServiceTicket>()
                .Where(x => x.TicketNumber.ToLower().Contains(value) || x.Title.ToLower().Contains(value))
                .Cast<TEntity>(),
            nameof(Notification) => queryable.Cast<Notification>()
                .Where(x => x.Title.ToLower().Contains(value) || x.Message.ToLower().Contains(value))
                .Cast<TEntity>(),
            nameof(AuditLog) => queryable.Cast<AuditLog>()
                .Where(x => x.EntityName.ToLower().Contains(value) || x.Action.ToLower().Contains(value))
                .Cast<TEntity>(),
            nameof(SystemSetting) => queryable.Cast<SystemSetting>()
                .Where(x => x.Key.ToLower().Contains(value))
                .Cast<TEntity>(),
            _ => queryable
        };
    }

    private static IQueryable<TEntity> ApplySorting<TEntity>(
        IQueryable<TEntity> queryable,
        string? sortBy,
        bool descending)
        where TEntity : class
    {
        if (typeof(BaseEntity).IsAssignableFrom(typeof(TEntity)))
        {
            return descending
                ? queryable.Cast<BaseEntity>().OrderByDescending(x => x.CreatedAtUtc).Cast<TEntity>()
                : queryable.Cast<BaseEntity>().OrderBy(x => x.CreatedAtUtc).Cast<TEntity>();
        }

        return queryable;
    }
}
