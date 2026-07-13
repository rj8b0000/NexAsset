using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IAssetReturnRepository
{
    Task AddAsync(AssetReturn assetReturn, CancellationToken cancellationToken);
    Task<List<AssetReturn>> GetHistoryByAssetIdAsync(Guid assetId, CancellationToken cancellationToken);
}
