using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Models.Procurement;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Procurement
{
    /// <summary>Real HTTP client for /api/enterprise-operations/purchase-orders.</summary>
    public sealed class PurchaseOrderApiClient : ApiClientBase, IPurchaseOrderApiClient
    {
        private const string BasePath = "api/enterprise-operations/purchase-orders";

        public PurchaseOrderApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<PurchaseOrderItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<PurchaseOrderItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<PurchaseOrderItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<PurchaseOrderItem>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(PurchaseOrderFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<PurchaseOrderFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> SetStatusAsync(Guid id, SetProcurementStatusRequest request, CancellationToken cancellationToken = default)
            => PostAsync($"{BasePath}/{id}/status", request, cancellationToken);
    }
}
