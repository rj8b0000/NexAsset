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
    /// <summary>Real HTTP client for /api/asset-categories.</summary>
    public sealed class AssetCategoryApiClient : ApiClientBase, IAssetCategoryApiClient
    {
        private const string BasePath = "api/asset-categories";

        public AssetCategoryApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<AssetCategoryListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<AssetCategoryListItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<AssetCategoryDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<AssetCategoryDetail>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(AssetCategoryFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<AssetCategoryFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, AssetCategoryFormModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/{id}", cancellationToken);
    }
}
