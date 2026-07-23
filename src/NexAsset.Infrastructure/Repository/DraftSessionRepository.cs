using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Repository;

public sealed class DraftSessionRepository : IDraftSessionRepository
{
    private readonly ApplicationDbContext _context;

    public DraftSessionRepository(ApplicationDbContext context) => _context = context;

    public Task<DraftSession?> GetByUserAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken) =>
        _context.DraftSessions.FirstOrDefaultAsync(x => x.UserId == userId && x.OrganizationId == organizationId && !x.IsDeleted, cancellationToken);

    public async Task AddAsync(DraftSession draftSession, CancellationToken cancellationToken) =>
        await _context.DraftSessions.AddAsync(draftSession, cancellationToken);

    public void Update(DraftSession draftSession) => _context.DraftSessions.Update(draftSession);

    public void Remove(DraftSession draftSession) => _context.DraftSessions.Remove(draftSession);
}
