using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IProjectCategoryRepository
{
    Task<bool> ExistsByNameAsync(Guid organizationId, string name, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(Guid organizationId, string name, Guid excludeId, CancellationToken cancellationToken);
    Task AddAsync(ProjectCategory category, CancellationToken cancellationToken);
    Task<ProjectCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResponse<ProjectCategory>> GetPagedAsync(PagedRequest request, bool? isActive, CancellationToken cancellationToken);
    void Update(ProjectCategory category);
}
