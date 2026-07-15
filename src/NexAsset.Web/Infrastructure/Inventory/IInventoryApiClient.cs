using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Models.Inventory;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Inventory
{
    /// <summary>
    /// Typed client for /api/enterprise-operations/inventory: item CRUD (no delete on the backend),
    /// stock movements, and per-item stock history.
    /// </summary>
    public interface IInventoryApiClient
    {
        Task<ApiResult<PagedResult<InventoryItemModel>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<InventoryItemModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> CreateAsync(InventoryItemFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateAsync(Guid id, InventoryItemFormModel model, CancellationToken cancellationToken = default);

        Task<ApiResult> RecordMovementAsync(StockMovementFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult<List<StockMovementRecord>>> GetStockHistoryAsync(Guid inventoryItemId, CancellationToken cancellationToken = default);
    }
}
