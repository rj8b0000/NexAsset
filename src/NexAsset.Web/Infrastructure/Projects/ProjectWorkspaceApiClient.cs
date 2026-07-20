using System.Net.Http;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Projects
{
    public sealed class ProjectWorkspaceApiClient : ApiClientBase, IProjectWorkspaceApiClient
    {
        private const string BasePath = "api/project-workspace";

        public ProjectWorkspaceApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<ProjectCategoryItem>>> GetCategoriesAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<ProjectCategoryItem>>(QueryStringBuilder.ForPagedQuery($"{BasePath}/categories", query), cancellationToken);

        public Task<ApiResult<ProjectCategoryItem>> CreateCategoryAsync(ProjectCategoryItem model, CancellationToken cancellationToken = default)
            => PostAsync<ProjectCategoryItem, ProjectCategoryItem>($"{BasePath}/categories", model, cancellationToken);

        public Task<ApiResult<ProjectCategoryItem>> UpdateCategoryAsync(Guid id, ProjectCategoryItem model, CancellationToken cancellationToken = default)
            => PutAsync<ProjectCategoryItem, ProjectCategoryItem>($"{BasePath}/categories/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/categories/{id}", cancellationToken);

        public Task<ApiResult<PagedResult<ProjectItem>>> GetProjectsAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<ProjectItem>>(QueryStringBuilder.ForPagedQuery($"{BasePath}/projects", query), cancellationToken);

        public Task<ApiResult<ProjectItem>> GetProjectAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<ProjectItem>($"{BasePath}/projects/{id}", cancellationToken);

        public Task<ApiResult<ProjectItem>> CreateProjectAsync(ProjectFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<ProjectFormModel, ProjectItem>($"{BasePath}/projects", model, cancellationToken);

        public Task<ApiResult<ProjectItem>> UpdateProjectAsync(Guid id, ProjectFormModel model, CancellationToken cancellationToken = default)
            => PutAsync<ProjectFormModel, ProjectItem>($"{BasePath}/projects/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteProjectAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/projects/{id}", cancellationToken);

        public Task<ApiResult<ProjectItem>> ArchiveProjectAsync(Guid id, CancellationToken cancellationToken = default)
            => PostAsync<object, ProjectItem>($"{BasePath}/projects/{id}/archive", new { }, cancellationToken);

        public Task<ApiResult<ProjectItem>> DuplicateProjectAsync(Guid id, string projectName, string startDate, CancellationToken cancellationToken = default)
            => PostAsync<object, ProjectItem>($"{BasePath}/projects/{id}/duplicate", new { id, projectName, startDate }, cancellationToken);

        public Task<ApiResult<ProjectDraftItem>> SaveDraftAsync(ProjectDraftFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<ProjectDraftFormModel, ProjectDraftItem>($"{BasePath}/drafts", model, cancellationToken);

        public Task<ApiResult<IReadOnlyList<ProjectMemberItem>>> GetMembersAsync(Guid projectId, CancellationToken cancellationToken = default)
            => GetAsync<IReadOnlyList<ProjectMemberItem>>($"{BasePath}/projects/{projectId}/members", cancellationToken);

        public Task<ApiResult<ProjectMemberItem>> AddMemberAsync(Guid projectId, ProjectMemberFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<ProjectMemberFormModel, ProjectMemberItem>($"{BasePath}/projects/{projectId}/members", model, cancellationToken);

        public Task<ApiResult<IReadOnlyList<ProjectAssetAllocationItem>>> GetAssetAllocationsAsync(Guid projectId, CancellationToken cancellationToken = default)
            => GetAsync<IReadOnlyList<ProjectAssetAllocationItem>>($"{BasePath}/projects/{projectId}/asset-allocations", cancellationToken);

        public Task<ApiResult<ProjectAssetAllocationItem>> AllocateAssetAsync(Guid projectId, ProjectAssetAllocationFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<ProjectAssetAllocationFormModel, ProjectAssetAllocationItem>($"{BasePath}/projects/{projectId}/asset-allocations", model, cancellationToken);

        public Task<ApiResult<IReadOnlyList<ProjectParameterGroupItem>>> GetParameterGroupsAsync(Guid projectId, CancellationToken cancellationToken = default)
            => GetAsync<IReadOnlyList<ProjectParameterGroupItem>>($"{BasePath}/projects/{projectId}/parameter-groups", cancellationToken);

        public Task<ApiResult<ProjectParameterGroupItem>> CreateParameterGroupAsync(Guid projectId, ProjectParameterGroupFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<ProjectParameterGroupFormModel, ProjectParameterGroupItem>($"{BasePath}/projects/{projectId}/parameter-groups", model, cancellationToken);

        public Task<ApiResult<IReadOnlyList<ProjectParameterItem>>> GetParametersAsync(Guid projectId, CancellationToken cancellationToken = default)
            => GetAsync<IReadOnlyList<ProjectParameterItem>>($"{BasePath}/projects/{projectId}/parameters", cancellationToken);

        public Task<ApiResult<ProjectParameterItem>> CreateParameterAsync(Guid projectId, ProjectParameterFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<ProjectParameterFormModel, ProjectParameterItem>($"{BasePath}/projects/{projectId}/parameters", model, cancellationToken);

        public Task<ApiResult<IReadOnlyList<ProjectDocumentItem>>> GetDocumentsAsync(Guid projectId, CancellationToken cancellationToken = default)
            => GetAsync<IReadOnlyList<ProjectDocumentItem>>($"{BasePath}/projects/{projectId}/documents", cancellationToken);

        public Task<ApiResult<ProjectDocumentItem>> AddDocumentAsync(Guid projectId, ProjectDocumentFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<ProjectDocumentFormModel, ProjectDocumentItem>($"{BasePath}/projects/{projectId}/documents", model, cancellationToken);

        public Task<ApiResult<IReadOnlyList<ProjectActivityItem>>> GetActivitiesAsync(Guid projectId, CancellationToken cancellationToken = default)
            => GetAsync<IReadOnlyList<ProjectActivityItem>>($"{BasePath}/projects/{projectId}/activities", cancellationToken);

        public Task<ApiResult<ProjectDashboardKpiItem>> GetDashboardKpisAsync(Guid projectId, CancellationToken cancellationToken = default)
            => GetAsync<ProjectDashboardKpiItem>($"{BasePath}/projects/{projectId}/dashboard-kpis", cancellationToken);

        public Task<ApiResult<ProjectBudgetItem>> GetBudgetAsync(Guid projectId, CancellationToken cancellationToken = default)
            => GetAsync<ProjectBudgetItem>($"{BasePath}/projects/{projectId}/budget", cancellationToken);

        public Task<ApiResult<ProjectBudgetItem>> UpsertBudgetAsync(Guid projectId, ProjectBudgetFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<ProjectBudgetFormModel, ProjectBudgetItem>($"{BasePath}/projects/{projectId}/budget", model, cancellationToken);

        public Task<ApiResult<IReadOnlyList<ProjectRiskItem>>> GetRisksAsync(Guid projectId, CancellationToken cancellationToken = default)
            => GetAsync<IReadOnlyList<ProjectRiskItem>>($"{BasePath}/projects/{projectId}/risks", cancellationToken);

        public Task<ApiResult<ProjectRiskItem>> CreateRiskAsync(Guid projectId, ProjectRiskFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<ProjectRiskFormModel, ProjectRiskItem>($"{BasePath}/projects/{projectId}/risks", model, cancellationToken);

        public Task<ApiResult<ProjectRiskItem>> UpdateRiskAsync(Guid id, ProjectRiskFormModel model, CancellationToken cancellationToken = default)
            => PutAsync<ProjectRiskFormModel, ProjectRiskItem>($"{BasePath}/risks/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteRiskAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/risks/{id}", cancellationToken);

        public Task<ApiResult<IReadOnlyList<ProjectSettingItem>>> GetSettingsAsync(Guid projectId, CancellationToken cancellationToken = default)
            => GetAsync<IReadOnlyList<ProjectSettingItem>>($"{BasePath}/projects/{projectId}/settings", cancellationToken);

        public Task<ApiResult<ProjectSettingItem>> UpsertSettingAsync(Guid projectId, ProjectSettingFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<ProjectSettingFormModel, ProjectSettingItem>($"{BasePath}/projects/{projectId}/settings", model, cancellationToken);
    }
}
