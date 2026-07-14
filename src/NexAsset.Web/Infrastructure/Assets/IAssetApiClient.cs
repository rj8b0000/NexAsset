using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Assets;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Assets
{
    /// <summary>
    /// Real typed client for /api/assets. In the Infrastructure.Assets namespace and referenced
    /// fully-qualified by the Asset pages, because the legacy mock
    /// <c>NexAsset.Web.Infrastructure.Services.IAssetApiClient</c> is still used by the out-of-scope
    /// Dashboard/Procurement/Maintenance mock flows (via MockDatabaseService.Assets).
    /// </summary>
    public interface IAssetApiClient
    {
        Task<ApiResult<PagedResult<AssetListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<AssetDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> CreateAsync(AssetFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateAsync(Guid id, AssetFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
