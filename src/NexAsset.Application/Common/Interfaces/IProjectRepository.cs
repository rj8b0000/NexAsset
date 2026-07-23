using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Common.Interfaces;

public interface IProjectRepository
{
    Task<bool> ExistsByCodeAsync(Guid organizationId, string projectCode, CancellationToken cancellationToken);
    Task<bool> ExistsByCodeAsync(Guid organizationId, string projectCode, Guid excludeId, CancellationToken cancellationToken);
    Task AddAsync(Project project, CancellationToken cancellationToken);
    Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Project?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResponse<Project>> GetPagedAsync(
        PagedRequest request,
        ProjectStatus? status,
        ProjectPriority? priority,
        Guid? categoryId,
        Guid? branchId,
        Guid? departmentId,
        Guid? projectManagerEmployeeId,
        CancellationToken cancellationToken);
    void Update(Project project);
}
