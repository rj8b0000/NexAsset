using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class AssetReturnRepository : IAssetReturnRepository
{
    private readonly ApplicationDbContext _context;
    public AssetReturnRepository(ApplicationDbContext context) => _context = context;

    public async Task AddAsync(AssetReturn assetReturn, CancellationToken cancellationToken) =>
        await _context.AssetReturns.AddAsync(assetReturn, cancellationToken);

    public Task<List<AssetReturn>> GetHistoryByAssetIdAsync(Guid assetId, CancellationToken cancellationToken) =>
        _context.AssetReturns.AsNoTracking().Where(x => x.AssetId == assetId && !x.IsDeleted).OrderByDescending(x => x.ReturnDate).ToListAsync(cancellationToken);
}
