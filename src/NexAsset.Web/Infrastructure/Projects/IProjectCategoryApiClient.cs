using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Projects;

public interface IProjectCategoryApiClient
{
    Task<ApiResult<PagedResult<ProjectCategoryItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectCategoryItem>> CreateAsync(ProjectCategoryInput input, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectCategoryItem>> UpdateAsync(Guid id, ProjectCategoryInput input, CancellationToken cancellationToken = default);
    Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
