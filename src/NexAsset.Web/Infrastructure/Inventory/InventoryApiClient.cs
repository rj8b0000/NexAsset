using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Models.Inventory;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Inventory
{
    /// <summary>Real HTTP client for /api/enterprise-operations/inventory.</summary>
    public sealed class InventoryApiClient : ApiClientBase, IInventoryApiClient
    {
        private const string BasePath = "api/enterprise-operations/inventory";

        public InventoryApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<InventoryItemModel>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<InventoryItemModel>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<InventoryItemModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<InventoryItemModel>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(InventoryItemFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<InventoryItemFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, InventoryItemFormModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);

        public Task<ApiResult> RecordMovementAsync(StockMovementFormModel model, CancellationToken cancellationToken = default)
            => PostAsync($"{BasePath}/stock-movements", model, cancellationToken);

        public Task<ApiResult<List<StockMovementRecord>>> GetStockHistoryAsync(Guid inventoryItemId, CancellationToken cancellationToken = default)
            => GetAsync<List<StockMovementRecord>>($"{BasePath}/{inventoryItemId}/stock-history", cancellationToken);
    }
}
