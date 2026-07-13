using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IAssetRepository
{
    Task<bool> ExistsByCodeAsync(Guid organizationId, string assetCode, CancellationToken cancellationToken);
    Task<bool> ExistsByCodeAsync(Guid organizationId, string assetCode, Guid excludeId, CancellationToken cancellationToken);
    Task<bool> ExistsBySerialNumberAsync(string serialNumber, CancellationToken cancellationToken);
    Task<bool> ExistsBySerialNumberAsync(string serialNumber, Guid excludeId, CancellationToken cancellationToken);
    Task AddAsync(Asset asset, CancellationToken cancellationToken);
    Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResponse<Asset>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken);
    void Update(Asset asset);
}
