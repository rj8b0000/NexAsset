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
        where TEntity : BaseEntity
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
        where TEntity : BaseEntity
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync<TEntity>(
        Guid id,
        CancellationToken cancellationToken)
        where TEntity : BaseEntity
    {
        return await _context.Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<PagedResponse<TEntity>> GetPagedAsync<TEntity>(
        PagedRequest request,
        CancellationToken cancellationToken)
        where TEntity : BaseEntity
    {
        IQueryable<TEntity> queryable = _context.Set<TEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

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
        where TEntity : BaseEntity
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
        where TEntity : BaseEntity
    {
        return filter(_context.Set<TEntity>().AsNoTracking())
            .CountAsync(cancellationToken);
    }

    private static IQueryable<TEntity> ApplySearch<TEntity>(
        IQueryable<TEntity> queryable,
        string? search)
        where TEntity : BaseEntity
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
        where TEntity : BaseEntity
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return SortByCreatedAt(queryable, descending);
        }

        var normalizedSort = sortBy.Trim().ToLowerInvariant();

        return typeof(TEntity).Name switch
        {
            nameof(Vendor) => SortVendor(queryable.Cast<Vendor>(), normalizedSort, descending).Cast<TEntity>(),
            nameof(Customer) => SortCustomer(queryable.Cast<Customer>(), normalizedSort, descending).Cast<TEntity>(),
            nameof(PurchaseRequest) => SortPurchaseRequest(queryable.Cast<PurchaseRequest>(), normalizedSort, descending).Cast<TEntity>(),
            nameof(PurchaseOrder) => SortPurchaseOrder(queryable.Cast<PurchaseOrder>(), normalizedSort, descending).Cast<TEntity>(),
            nameof(InventoryItem) => SortInventoryItem(queryable.Cast<InventoryItem>(), normalizedSort, descending).Cast<TEntity>(),
            nameof(Consumable) => SortConsumable(queryable.Cast<Consumable>(), normalizedSort, descending).Cast<TEntity>(),
            nameof(MaintenanceRecord) => SortMaintenanceRecord(queryable.Cast<MaintenanceRecord>(), normalizedSort, descending).Cast<TEntity>(),
            nameof(ServiceTicket) => SortServiceTicket(queryable.Cast<ServiceTicket>(), normalizedSort, descending).Cast<TEntity>(),
            nameof(Notification) => SortNotification(queryable.Cast<Notification>(), normalizedSort, descending).Cast<TEntity>(),
            nameof(AuditLog) => SortAuditLog(queryable.Cast<AuditLog>(), normalizedSort, descending).Cast<TEntity>(),
            nameof(SystemSetting) => SortSystemSetting(queryable.Cast<SystemSetting>(), normalizedSort, descending).Cast<TEntity>(),
            _ => SortByCreatedAt(queryable, descending)
        };
    }

    private static IQueryable<TEntity> SortByCreatedAt<TEntity>(
        IQueryable<TEntity> queryable,
        bool descending)
        where TEntity : BaseEntity
    {
        return descending
            ? queryable.OrderByDescending(x => x.CreatedAtUtc)
            : queryable.OrderBy(x => x.CreatedAtUtc);
    }

    private static IQueryable<Vendor> SortVendor(IQueryable<Vendor> queryable, string sortBy, bool descending) =>
        sortBy switch
        {
            "code" => descending ? queryable.OrderByDescending(x => x.Code) : queryable.OrderBy(x => x.Code),
            "name" => descending ? queryable.OrderByDescending(x => x.Name) : queryable.OrderBy(x => x.Name),
            "email" => descending ? queryable.OrderByDescending(x => x.Email) : queryable.OrderBy(x => x.Email),
            _ => SortByCreatedAt(queryable, descending)
        };

    private static IQueryable<Customer> SortCustomer(IQueryable<Customer> queryable, string sortBy, bool descending) =>
        sortBy switch
        {
            "code" => descending ? queryable.OrderByDescending(x => x.Code) : queryable.OrderBy(x => x.Code),
            "name" => descending ? queryable.OrderByDescending(x => x.Name) : queryable.OrderBy(x => x.Name),
            "email" => descending ? queryable.OrderByDescending(x => x.Email) : queryable.OrderBy(x => x.Email),
            _ => SortByCreatedAt(queryable, descending)
        };

    private static IQueryable<PurchaseRequest> SortPurchaseRequest(IQueryable<PurchaseRequest> queryable, string sortBy, bool descending) =>
        sortBy switch
        {
            "requestnumber" => descending ? queryable.OrderByDescending(x => x.RequestNumber) : queryable.OrderBy(x => x.RequestNumber),
            "title" => descending ? queryable.OrderByDescending(x => x.Title) : queryable.OrderBy(x => x.Title),
            "estimatedamount" => descending ? queryable.OrderByDescending(x => x.EstimatedAmount) : queryable.OrderBy(x => x.EstimatedAmount),
            _ => SortByCreatedAt(queryable, descending)
        };

    private static IQueryable<PurchaseOrder> SortPurchaseOrder(IQueryable<PurchaseOrder> queryable, string sortBy, bool descending) =>
        sortBy switch
        {
            "ordernumber" => descending ? queryable.OrderByDescending(x => x.OrderNumber) : queryable.OrderBy(x => x.OrderNumber),
            "totalamount" => descending ? queryable.OrderByDescending(x => x.TotalAmount) : queryable.OrderBy(x => x.TotalAmount),
            _ => SortByCreatedAt(queryable, descending)
        };

    private static IQueryable<InventoryItem> SortInventoryItem(IQueryable<InventoryItem> queryable, string sortBy, bool descending) =>
        sortBy switch
        {
            "itemcode" => descending ? queryable.OrderByDescending(x => x.ItemCode) : queryable.OrderBy(x => x.ItemCode),
            "itemname" => descending ? queryable.OrderByDescending(x => x.ItemName) : queryable.OrderBy(x => x.ItemName),
            "currentstock" => descending ? queryable.OrderByDescending(x => x.CurrentStock) : queryable.OrderBy(x => x.CurrentStock),
            _ => SortByCreatedAt(queryable, descending)
        };

    private static IQueryable<Consumable> SortConsumable(IQueryable<Consumable> queryable, string sortBy, bool descending) =>
        sortBy switch
        {
            "consumablecode" => descending ? queryable.OrderByDescending(x => x.ConsumableCode) : queryable.OrderBy(x => x.ConsumableCode),
            "name" => descending ? queryable.OrderByDescending(x => x.Name) : queryable.OrderBy(x => x.Name),
            _ => SortByCreatedAt(queryable, descending)
        };

    private static IQueryable<MaintenanceRecord> SortMaintenanceRecord(IQueryable<MaintenanceRecord> queryable, string sortBy, bool descending) =>
        sortBy switch
        {
            "title" => descending ? queryable.OrderByDescending(x => x.Title) : queryable.OrderBy(x => x.Title),
            "cost" => descending ? queryable.OrderByDescending(x => x.Cost) : queryable.OrderBy(x => x.Cost),
            _ => SortByCreatedAt(queryable, descending)
        };

    private static IQueryable<ServiceTicket> SortServiceTicket(IQueryable<ServiceTicket> queryable, string sortBy, bool descending) =>
        sortBy switch
        {
            "ticketnumber" => descending ? queryable.OrderByDescending(x => x.TicketNumber) : queryable.OrderBy(x => x.TicketNumber),
            "title" => descending ? queryable.OrderByDescending(x => x.Title) : queryable.OrderBy(x => x.Title),
            _ => SortByCreatedAt(queryable, descending)
        };

    private static IQueryable<Notification> SortNotification(IQueryable<Notification> queryable, string sortBy, bool descending) =>
        sortBy switch
        {
            "title" => descending ? queryable.OrderByDescending(x => x.Title) : queryable.OrderBy(x => x.Title),
            _ => SortByCreatedAt(queryable, descending)
        };

    private static IQueryable<AuditLog> SortAuditLog(IQueryable<AuditLog> queryable, string sortBy, bool descending) =>
        sortBy switch
        {
            "entityname" => descending ? queryable.OrderByDescending(x => x.EntityName) : queryable.OrderBy(x => x.EntityName),
            "action" => descending ? queryable.OrderByDescending(x => x.Action) : queryable.OrderBy(x => x.Action),
            _ => SortByCreatedAt(queryable, descending)
        };

    private static IQueryable<SystemSetting> SortSystemSetting(IQueryable<SystemSetting> queryable, string sortBy, bool descending) =>
        sortBy switch
        {
            "key" => descending ? queryable.OrderByDescending(x => x.Key) : queryable.OrderBy(x => x.Key),
            _ => SortByCreatedAt(queryable, descending)
        };
}
