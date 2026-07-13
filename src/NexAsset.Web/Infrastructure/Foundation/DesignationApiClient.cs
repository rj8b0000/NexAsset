using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Foundation
{
    /// <summary>Real HTTP client for /api/designations.</summary>
    public sealed class DesignationApiClient : ApiClientBase, IDesignationApiClient
    {
        private const string BasePath = "api/designations";

        public DesignationApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<DesignationListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<DesignationListItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<DesignationDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<DesignationDetail>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(DesignationFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<DesignationFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, DesignationFormModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/{id}", cancellationToken);
    }
}
