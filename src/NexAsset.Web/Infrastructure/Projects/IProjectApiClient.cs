using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Projects;

public interface IProjectApiClient
{
    Task<ApiResult<PagedResult<ProjectListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectDetail>> CreateAsync(ProjectInput input, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectDetail>> UpdateAsync(Guid id, ProjectInput input, CancellationToken cancellationToken = default);
    Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApiResult<ProjectDashboard>> GetDashboardAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApiResult<PagedResult<TeamMember>>> GetTeamAsync(Guid id, PagedQuery query, CancellationToken cancellationToken = default);
    Task<ApiResult<PagedResult<AssetAllocation>>> GetAssetsAsync(Guid id, PagedQuery query, CancellationToken cancellationToken = default);
    Task<ApiResult<PagedResult<ProjectDocument>>> GetDocumentsAsync(Guid id, PagedQuery query, CancellationToken cancellationToken = default);
    Task<ApiResult<PagedResult<ProjectRisk>>> GetRisksAsync(Guid id, PagedQuery query, CancellationToken cancellationToken = default);
    Task<ApiResult<IReadOnlyCollection<ParameterSection>>> GetParametersAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApiResult<BudgetSnapshot>> GetBudgetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApiResult<DraftInput>> GetDraftAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<ApiResult<DraftInput>> SaveDraftAsync(DraftInput input, CancellationToken cancellationToken = default);
}
