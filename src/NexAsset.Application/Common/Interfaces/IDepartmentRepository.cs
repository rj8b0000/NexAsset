using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IDepartmentRepository
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
        Department department,
        CancellationToken cancellationToken);

    Task<Department?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResponse<Department>> GetPagedAsync(
        PagedRequest request,
        CancellationToken cancellationToken);

    void Update(Department department);
}
