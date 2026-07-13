using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class AssetAssignmentRepository : IAssetAssignmentRepository
{
    private readonly ApplicationDbContext _context;
    public AssetAssignmentRepository(ApplicationDbContext context) => _context = context;

    public async Task AddAsync(AssetAssignment assignment, CancellationToken cancellationToken) =>
        await _context.AssetAssignments.AddAsync(assignment, cancellationToken);

    public Task<AssetAssignment?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _context.AssetAssignments.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public Task<AssetAssignment?> GetCurrentByAssetIdAsync(Guid assetId, CancellationToken cancellationToken) =>
        _context.AssetAssignments.FirstOrDefaultAsync(x => x.AssetId == assetId && x.Status == AssetAssignmentStatus.Active && !x.IsDeleted, cancellationToken);

    public Task<List<AssetAssignment>> GetHistoryByAssetIdAsync(Guid assetId, CancellationToken cancellationToken) =>
        _context.AssetAssignments.AsNoTracking().Where(x => x.AssetId == assetId && !x.IsDeleted).OrderByDescending(x => x.AssignedDate).ToListAsync(cancellationToken);

    public void Update(AssetAssignment assignment) => _context.AssetAssignments.Update(assignment);
}
