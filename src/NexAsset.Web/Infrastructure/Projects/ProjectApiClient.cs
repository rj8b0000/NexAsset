using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Projects;

public sealed class ProjectApiClient : ApiClientBase, IProjectApiClient
{
    private const string BasePath = "api/projects";

    public ProjectApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<ApiResult<PagedResult<ProjectListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
        => GetAsync<PagedResult<ProjectListItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

    public Task<ApiResult<ProjectDetailModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetAsync<ProjectDetailModel>($"{BasePath}/{id}", cancellationToken);

    public Task<ApiResult<ProjectDetailModel>> CreateAsync(ProjectFormModel model, CancellationToken cancellationToken = default)
        => PostAsync<ProjectFormModel, ProjectDetailModel>(BasePath, model, cancellationToken);

    public Task<ApiResult<ProjectDetailModel>> UpdateAsync(Guid id, ProjectFormModel model, CancellationToken cancellationToken = default)
        => PutAsync<ProjectFormModel, ProjectDetailModel>($"{BasePath}/{id}", model, cancellationToken);

    public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => DeleteAsync($"{BasePath}/{id}", cancellationToken);

    public Task<ApiResult<ProjectDetailModel>> TransitionStatusAsync(Guid id, TransitionStatusModel model, CancellationToken cancellationToken = default)
        => PostAsync<TransitionStatusModel, ProjectDetailModel>($"{BasePath}/{id}/transition-status", model, cancellationToken);

    public Task<ApiResult<ProjectDetailModel>> DuplicateAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
        => PostAsync<object, ProjectDetailModel>($"{BasePath}/{id}/duplicate?organizationId={organizationId}", new { }, cancellationToken);

    public Task<ApiResult<DraftSessionModel>> SaveDraftSessionAsync(DraftSessionModel model, CancellationToken cancellationToken = default)
        => PostAsync<DraftSessionModel, DraftSessionModel>($"{BasePath}/draft-sessions", model, cancellationToken);

    public Task<ApiResult<DraftSessionModel?>> GetDraftSessionAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken = default)
        => GetAsync<DraftSessionModel?>($"{BasePath}/draft-sessions?userId={userId}&organizationId={organizationId}", cancellationToken);

    public Task<ApiResult> DeleteDraftSessionAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken = default)
        => DeleteAsync($"{BasePath}/draft-sessions?userId={userId}&organizationId={organizationId}", cancellationToken);
}
