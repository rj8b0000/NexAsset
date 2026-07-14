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
    /// <summary>Real HTTP client for /api/enterprise-operations/purchase-requests.</summary>
    public sealed class PurchaseRequestApiClient : ApiClientBase, IPurchaseRequestApiClient
    {
        private const string BasePath = "api/enterprise-operations/purchase-requests";

        public PurchaseRequestApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<PurchaseRequestItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<PurchaseRequestItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<PurchaseRequestItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<PurchaseRequestItem>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(PurchaseRequestFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<PurchaseRequestFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> SetStatusAsync(Guid id, SetProcurementStatusRequest request, CancellationToken cancellationToken = default)
            => PostAsync($"{BasePath}/{id}/status", request, cancellationToken);
    }
}
