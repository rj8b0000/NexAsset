using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class AssetTransferRepository : IAssetTransferRepository
{
    private readonly ApplicationDbContext _context;
    public AssetTransferRepository(ApplicationDbContext context) => _context = context;

    public async Task AddAsync(AssetTransfer transfer, CancellationToken cancellationToken) =>
        await _context.AssetTransfers.AddAsync(transfer, cancellationToken);

    public Task<List<AssetTransfer>> GetHistoryByAssetIdAsync(Guid assetId, CancellationToken cancellationToken) =>
        _context.AssetTransfers.AsNoTracking().Where(x => x.AssetId == assetId && !x.IsDeleted).OrderByDescending(x => x.TransferDate).ToListAsync(cancellationToken);
}
