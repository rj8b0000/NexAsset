using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IAssetAssignmentRepository
{
    Task AddAsync(AssetAssignment assignment, CancellationToken cancellationToken);
    Task<AssetAssignment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<AssetAssignment?> GetCurrentByAssetIdAsync(Guid assetId, CancellationToken cancellationToken);
    Task<List<AssetAssignment>> GetHistoryByAssetIdAsync(Guid assetId, CancellationToken cancellationToken);
    void Update(AssetAssignment assignment);
}
