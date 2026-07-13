using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IDesignationRepository
{
    Task<bool> ExistsByTitleAsync(
        Guid organizationId,
        string title,
        CancellationToken cancellationToken);

    Task<bool> ExistsByTitleAsync(
        Guid organizationId,
        string title,
        Guid excludeId,
        CancellationToken cancellationToken);

    Task AddAsync(
        Designation designation,
        CancellationToken cancellationToken);

    Task<Designation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResponse<Designation>> GetPagedAsync(
        PagedRequest request,
        CancellationToken cancellationToken);

    void Update(Designation designation);
}
