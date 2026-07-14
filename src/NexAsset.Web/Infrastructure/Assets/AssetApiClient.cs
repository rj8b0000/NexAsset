using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Assets;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Assets
{
    /// <summary>Real HTTP client for /api/assets.</summary>
    public sealed class AssetApiClient : ApiClientBase, IAssetApiClient
    {
        private const string BasePath = "api/assets";

        public AssetApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<AssetListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<AssetListItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<AssetDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<AssetDetail>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(AssetFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<AssetFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, AssetFormModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/{id}", cancellationToken);
    }
}
