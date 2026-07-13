using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IBranchRepository
{
    Task<bool> ExistsByCodeAsync(
        Guid organizationId,
        string code,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        Guid organizationId,
        string code,
        Guid excludeId,
        CancellationToken cancellationToken);

    Task AddAsync(
        Branch branch,
        CancellationToken cancellationToken);

    Task<Branch?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResponse<Branch>> GetPagedAsync(
        PagedRequest request,
        CancellationToken cancellationToken);

    void Update(Branch branch);
}
