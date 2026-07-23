using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Projects;

public interface IProjectApiClient
{
    Task<ApiResult<PagedResult<ProjectListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectDetailModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectDetailModel>> CreateAsync(ProjectFormModel model, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectDetailModel>> UpdateAsync(Guid id, ProjectFormModel model, CancellationToken cancellationToken = default);
    Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectDetailModel>> TransitionStatusAsync(Guid id, TransitionStatusModel model, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectDetailModel>> DuplicateAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<ApiResult<DraftSessionModel>> SaveDraftSessionAsync(DraftSessionModel model, CancellationToken cancellationToken = default);
    Task<ApiResult<DraftSessionModel?>> GetDraftSessionAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken = default);
    Task<ApiResult> DeleteDraftSessionAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken = default);
}
