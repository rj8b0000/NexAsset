using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IEnterpriseOperationsRepository
{
    Task<bool> ExistsAsync<TEntity>(Guid organizationId, string code, CancellationToken cancellationToken)
        where TEntity : class;

    Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
        where TEntity : class;

    Task<TEntity?> GetByIdAsync<TEntity>(Guid id, CancellationToken cancellationToken)
        where TEntity : class;

    Task<PagedResponse<TEntity>> GetPagedAsync<TEntity>(
        PagedRequest request,
        CancellationToken cancellationToken)
        where TEntity : class;

    void Update<TEntity>(TEntity entity)
        where TEntity : class;

    Task<InventoryItem?> GetInventoryItemAsync(Guid id, CancellationToken cancellationToken);
    Task<List<StockMovement>> GetStockHistoryAsync(Guid inventoryItemId, CancellationToken cancellationToken);
    Task<List<PurchaseRequest>> GetPurchaseRequestHistoryAsync(Guid organizationId, CancellationToken cancellationToken);
    Task<List<MaintenanceRecord>> GetMaintenanceHistoryAsync(Guid assetId, CancellationToken cancellationToken);
    Task<List<ServiceTicket>> GetTicketHistoryAsync(Guid customerId, CancellationToken cancellationToken);
    Task<SystemSetting?> GetSystemSettingAsync(Guid? organizationId, string key, CancellationToken cancellationToken);
    Task<int> CountAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> filter, CancellationToken cancellationToken)
        where TEntity : class;
}
