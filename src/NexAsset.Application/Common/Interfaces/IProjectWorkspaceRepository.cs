using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Common;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IProjectWorkspaceRepository
{
    Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
        where TEntity : BaseEntity;

    void Update<TEntity>(TEntity entity)
        where TEntity : BaseEntity;

    Task<TEntity?> GetByIdAsync<TEntity>(Guid id, CancellationToken cancellationToken)
        where TEntity : BaseEntity;

    Task<PagedResponse<ProjectCategory>> GetCategoriesAsync(PagedRequest request, CancellationToken cancellationToken);
    Task<PagedResponse<Project>> GetProjectsAsync(PagedRequest request, CancellationToken cancellationToken);
    Task<List<ProjectMember>> GetMembersAsync(Guid projectId, CancellationToken cancellationToken);
    Task<List<ProjectAssetAllocation>> GetAssetAllocationsAsync(Guid projectId, CancellationToken cancellationToken);
    Task<List<ProjectAssetAllocation>> GetAssetHistoryAsync(Guid assetId, CancellationToken cancellationToken);
    Task<List<ProjectParameterGroup>> GetParameterGroupsAsync(Guid projectId, CancellationToken cancellationToken);
    Task<List<ProjectParameter>> GetParametersAsync(Guid projectId, CancellationToken cancellationToken);
    Task<List<ProjectDocument>> GetDocumentsAsync(Guid projectId, CancellationToken cancellationToken);
    Task<List<ProjectActivity>> GetActivitiesAsync(Guid projectId, CancellationToken cancellationToken);
    Task<ProjectDraft?> GetDraftAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResponse<ProjectDraft>> GetDraftsAsync(PagedRequest request, CancellationToken cancellationToken);
    Task<ProjectBudget?> GetBudgetAsync(Guid projectId, CancellationToken cancellationToken);
    Task<List<ProjectRisk>> GetRisksAsync(Guid projectId, CancellationToken cancellationToken);
    Task<List<ProjectSetting>> GetSettingsAsync(Guid projectId, CancellationToken cancellationToken);
}
