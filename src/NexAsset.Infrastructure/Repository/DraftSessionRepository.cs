using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Repository;

public sealed class DraftSessionRepository : IDraftSessionRepository
{
    private readonly ApplicationDbContext _context;

    public DraftSessionRepository(ApplicationDbContext context) => _context = context;

    public Task<DraftSession?> GetByUserAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken)
    {
        IQueryable<DraftSession> query = _context.DraftSessions.AsNoTracking().Where(x => !x.IsDeleted);

        if (userId != Guid.Empty)
        {
            query = query.Where(x => x.UserId == userId);
        }

        if (organizationId != Guid.Empty)
        {
            query = query.Where(x => x.OrganizationId == organizationId);
        }

        return query.OrderByDescending(x => x.LastSavedAtUtc).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(DraftSession draftSession, CancellationToken cancellationToken) =>
        await _context.DraftSessions.AddAsync(draftSession, cancellationToken);

    public void Update(DraftSession draftSession) => _context.DraftSessions.Update(draftSession);

    public void Remove(DraftSession draftSession) => _context.DraftSessions.Remove(draftSession);
}
