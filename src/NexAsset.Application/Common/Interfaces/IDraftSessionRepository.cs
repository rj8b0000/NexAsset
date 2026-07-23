using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IDraftSessionRepository
{
    Task<DraftSession?> GetByUserAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken);
    Task AddAsync(DraftSession draftSession, CancellationToken cancellationToken);
    void Update(DraftSession draftSession);
    void Remove(DraftSession draftSession);
}
