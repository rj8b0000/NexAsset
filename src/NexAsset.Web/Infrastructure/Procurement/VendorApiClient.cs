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
    /// <summary>Real HTTP client for /api/enterprise-operations/vendors.</summary>
    public sealed class VendorApiClient : ApiClientBase, IVendorApiClient
    {
        private const string BasePath = "api/enterprise-operations/vendors";

        public VendorApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<VendorItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<VendorItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<VendorItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<VendorItem>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(VendorFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<VendorFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, VendorFormModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/{id}", cancellationToken);
    }
}
