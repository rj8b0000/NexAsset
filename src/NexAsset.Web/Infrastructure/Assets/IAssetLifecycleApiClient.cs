using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Assets;

namespace NexAsset.Web.Infrastructure.Assets
{
    /// <summary>
    /// Typed client for the asset lifecycle workflow: assign/unassign, transfer, return, and the
    /// three per-asset histories. The backend exposes these as actions (not CRUD lists), so this
    /// client is action-shaped too.
    /// </summary>
    public interface IAssetLifecycleApiClient
    {
        Task<ApiResult> AssignAsync(AssignAssetRequest request, CancellationToken cancellationToken = default);
        Task<ApiResult> UnassignAsync(UnassignAssetRequest request, CancellationToken cancellationToken = default);
        Task<ApiResult> TransferAsync(TransferAssetRequest request, CancellationToken cancellationToken = default);
        Task<ApiResult> ReturnAsync(ReturnAssetRequest request, CancellationToken cancellationToken = default);

        Task<ApiResult<List<AssetAssignmentRecord>>> GetAssignmentHistoryAsync(Guid assetId, CancellationToken cancellationToken = default);
        Task<ApiResult<List<AssetTransferRecord>>> GetTransferHistoryAsync(Guid assetId, CancellationToken cancellationToken = default);
        Task<ApiResult<List<AssetReturnRecord>>> GetReturnHistoryAsync(Guid assetId, CancellationToken cancellationToken = default);
    }
}
