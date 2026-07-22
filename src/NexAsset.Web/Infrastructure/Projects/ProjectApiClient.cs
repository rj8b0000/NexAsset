using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Projects;

public sealed class ProjectApiClient(HttpClient httpClient) : ApiClientBase(httpClient), IProjectApiClient
{
    private const string BasePath = "api/projects";

    public Task<ApiResult<PagedResult<ProjectListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default) => GetAsync<PagedResult<ProjectListItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);
    public Task<ApiResult<ProjectDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => GetAsync<ProjectDetail>($"{BasePath}/{id}", cancellationToken);
    public Task<ApiResult<ProjectDetail>> CreateAsync(ProjectInput input, CancellationToken cancellationToken = default) => PostAsync<ProjectInput, ProjectDetail>(BasePath, input, cancellationToken);
    public Task<ApiResult<ProjectDetail>> UpdateAsync(Guid id, ProjectInput input, CancellationToken cancellationToken = default) => PutAsync<ProjectInput, ProjectDetail>($"{BasePath}/{id}", input, cancellationToken);
    public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default) => DeleteAsync($"{BasePath}/{id}", cancellationToken);
    public Task<ApiResult<ProjectDashboard>> GetDashboardAsync(Guid id, CancellationToken cancellationToken = default) => GetAsync<ProjectDashboard>($"{BasePath}/{id}/dashboard", cancellationToken);
    public Task<ApiResult<PagedResult<TeamMember>>> GetTeamAsync(Guid id, PagedQuery query, CancellationToken cancellationToken = default) => GetAsync<PagedResult<TeamMember>>(QueryStringBuilder.ForPagedQuery($"{BasePath}/{id}/team", query), cancellationToken);
    public Task<ApiResult<PagedResult<AssetAllocation>>> GetAssetsAsync(Guid id, PagedQuery query, CancellationToken cancellationToken = default) => GetAsync<PagedResult<AssetAllocation>>(QueryStringBuilder.ForPagedQuery($"{BasePath}/{id}/assets", query), cancellationToken);
    public Task<ApiResult<PagedResult<ProjectDocument>>> GetDocumentsAsync(Guid id, PagedQuery query, CancellationToken cancellationToken = default) => GetAsync<PagedResult<ProjectDocument>>(QueryStringBuilder.ForPagedQuery($"{BasePath}/{id}/documents", query), cancellationToken);
    public Task<ApiResult<PagedResult<ProjectRisk>>> GetRisksAsync(Guid id, PagedQuery query, CancellationToken cancellationToken = default) => GetAsync<PagedResult<ProjectRisk>>(QueryStringBuilder.ForPagedQuery($"{BasePath}/{id}/risks", query), cancellationToken);
    public Task<ApiResult<IReadOnlyCollection<ParameterSection>>> GetParametersAsync(Guid id, CancellationToken cancellationToken = default) => GetAsync<IReadOnlyCollection<ParameterSection>>($"{BasePath}/{id}/parameters", cancellationToken);
    public Task<ApiResult<BudgetSnapshot>> GetBudgetAsync(Guid id, CancellationToken cancellationToken = default) => GetAsync<BudgetSnapshot>($"{BasePath}/{id}/budget", cancellationToken);
    public Task<ApiResult<DraftInput>> GetDraftAsync(Guid organizationId, CancellationToken cancellationToken = default) => GetAsync<DraftInput>($"{BasePath}/drafts?organizationId={organizationId}", cancellationToken);
    public Task<ApiResult<DraftInput>> SaveDraftAsync(DraftInput input, CancellationToken cancellationToken = default) => PutAsync<DraftInput, DraftInput>($"{BasePath}/drafts", input, cancellationToken);
}
