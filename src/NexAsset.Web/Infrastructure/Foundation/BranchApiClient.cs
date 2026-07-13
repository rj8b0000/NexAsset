using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Foundation
{
    /// <summary>Real HTTP client for /api/branches.</summary>
    public sealed class BranchApiClient : ApiClientBase, IBranchApiClient
    {
        private const string BasePath = "api/branches";

        public BranchApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<BranchListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<BranchListItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<BranchDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<BranchDetail>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(BranchFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<BranchFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, BranchFormModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/{id}", cancellationToken);
    }
}
