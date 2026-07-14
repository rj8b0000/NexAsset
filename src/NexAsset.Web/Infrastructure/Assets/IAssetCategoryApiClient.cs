using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Assets;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Assets
{
    /// <summary>Typed client for /api/asset-categories.</summary>
    public interface IAssetCategoryApiClient
    {
        Task<ApiResult<PagedResult<AssetCategoryListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<AssetCategoryDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> CreateAsync(AssetCategoryFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateAsync(Guid id, AssetCategoryFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
