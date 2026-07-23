using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Projects;

public interface IProjectCategoryApiClient
{
    Task<ApiResult<PagedResult<ProjectCategoryListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectCategoryListItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectCategoryListItem>> CreateAsync(ProjectCategoryFormModel model, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectCategoryListItem>> UpdateAsync(Guid id, ProjectCategoryFormModel model, CancellationToken cancellationToken = default);
    Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
