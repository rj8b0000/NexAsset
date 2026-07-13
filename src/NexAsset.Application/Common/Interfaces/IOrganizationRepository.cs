using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IOrganizationRepository
{
    Task<bool> ExistsByCodeAsync(
        string code,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        string code,
        Guid excludeId,
        CancellationToken cancellationToken);

    Task AddAsync(
        Organization organization,
        CancellationToken cancellationToken);

    Task<Organization?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<List<Organization>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<PagedResponse<Organization>> GetPagedAsync(
        PagedRequest request,
        CancellationToken cancellationToken);

    void Update(Organization organization);

    void Delete(Organization organization);
}