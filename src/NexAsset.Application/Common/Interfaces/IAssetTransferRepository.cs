using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IAssetTransferRepository
{
    Task AddAsync(AssetTransfer transfer, CancellationToken cancellationToken);
    Task<List<AssetTransfer>> GetHistoryByAssetIdAsync(Guid assetId, CancellationToken cancellationToken);
}
