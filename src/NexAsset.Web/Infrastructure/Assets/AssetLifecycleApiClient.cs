using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Assets;

namespace NexAsset.Web.Infrastructure.Assets
{
    /// <summary>Real HTTP client for asset assign/unassign/transfer/return + per-asset histories.</summary>
    public sealed class AssetLifecycleApiClient : ApiClientBase, IAssetLifecycleApiClient
    {
        public AssetLifecycleApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult> AssignAsync(AssignAssetRequest request, CancellationToken cancellationToken = default)
            => PostAsync("api/asset-assignments/assign", request, cancellationToken);

        public Task<ApiResult> UnassignAsync(UnassignAssetRequest request, CancellationToken cancellationToken = default)
            => PostAsync("api/asset-assignments/unassign", request, cancellationToken);

        public Task<ApiResult> TransferAsync(TransferAssetRequest request, CancellationToken cancellationToken = default)
            => PostAsync("api/asset-transfers", request, cancellationToken);

        public Task<ApiResult> ReturnAsync(ReturnAssetRequest request, CancellationToken cancellationToken = default)
            => PostAsync("api/asset-returns", request, cancellationToken);

        public Task<ApiResult<List<AssetAssignmentRecord>>> GetAssignmentHistoryAsync(Guid assetId, CancellationToken cancellationToken = default)
            => GetAsync<List<AssetAssignmentRecord>>($"api/asset-assignments/assets/{assetId}/history", cancellationToken);

        public Task<ApiResult<List<AssetTransferRecord>>> GetTransferHistoryAsync(Guid assetId, CancellationToken cancellationToken = default)
            => GetAsync<List<AssetTransferRecord>>($"api/asset-transfers/assets/{assetId}/history", cancellationToken);

        public Task<ApiResult<List<AssetReturnRecord>>> GetReturnHistoryAsync(Guid assetId, CancellationToken cancellationToken = default)
            => GetAsync<List<AssetReturnRecord>>($"api/asset-returns/assets/{assetId}/history", cancellationToken);
    }
}
