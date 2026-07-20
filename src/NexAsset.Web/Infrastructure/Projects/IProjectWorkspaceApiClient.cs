using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Projects
{
    public interface IProjectWorkspaceApiClient
    {
        Task<ApiResult<PagedResult<ProjectCategoryItem>>> GetCategoriesAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectCategoryItem>> CreateCategoryAsync(ProjectCategoryItem model, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectCategoryItem>> UpdateCategoryAsync(Guid id, ProjectCategoryItem model, CancellationToken cancellationToken = default);
        Task<ApiResult> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default);

        Task<ApiResult<PagedResult<ProjectItem>>> GetProjectsAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectItem>> GetProjectAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectItem>> CreateProjectAsync(ProjectFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectItem>> UpdateProjectAsync(Guid id, ProjectFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> DeleteProjectAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectItem>> ArchiveProjectAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectItem>> DuplicateProjectAsync(Guid id, string projectName, string startDate, CancellationToken cancellationToken = default);

        Task<ApiResult<ProjectDraftItem>> SaveDraftAsync(ProjectDraftFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult<IReadOnlyList<ProjectMemberItem>>> GetMembersAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectMemberItem>> AddMemberAsync(Guid projectId, ProjectMemberFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult<IReadOnlyList<ProjectAssetAllocationItem>>> GetAssetAllocationsAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectAssetAllocationItem>> AllocateAssetAsync(Guid projectId, ProjectAssetAllocationFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult<IReadOnlyList<ProjectParameterGroupItem>>> GetParameterGroupsAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectParameterGroupItem>> CreateParameterGroupAsync(Guid projectId, ProjectParameterGroupFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult<IReadOnlyList<ProjectParameterItem>>> GetParametersAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectParameterItem>> CreateParameterAsync(Guid projectId, ProjectParameterFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult<IReadOnlyList<ProjectDocumentItem>>> GetDocumentsAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectDocumentItem>> AddDocumentAsync(Guid projectId, ProjectDocumentFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult<IReadOnlyList<ProjectActivityItem>>> GetActivitiesAsync(Guid projectId, CancellationToken cancellationToken = default);

        Task<ApiResult<ProjectDashboardKpiItem>> GetDashboardKpisAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectBudgetItem>> GetBudgetAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectBudgetItem>> UpsertBudgetAsync(Guid projectId, ProjectBudgetFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult<IReadOnlyList<ProjectRiskItem>>> GetRisksAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectRiskItem>> CreateRiskAsync(Guid projectId, ProjectRiskFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectRiskItem>> UpdateRiskAsync(Guid id, ProjectRiskFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> DeleteRiskAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<IReadOnlyList<ProjectSettingItem>>> GetSettingsAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<ApiResult<ProjectSettingItem>> UpsertSettingAsync(Guid projectId, ProjectSettingFormModel model, CancellationToken cancellationToken = default);
    }
}
