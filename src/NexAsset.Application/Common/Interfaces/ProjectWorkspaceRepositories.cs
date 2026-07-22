using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Features.Projects;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Common.Interfaces;

public interface IProjectCategoryRepository
{
    Task<bool> ExistsByNameAsync(Guid organizationId, string name, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(Guid organizationId, string name, Guid excludeId, CancellationToken cancellationToken);
    Task AddAsync(ProjectCategory category, CancellationToken cancellationToken);
    Task<ProjectCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResponse<ProjectCategory>> GetPagedAsync(PagedRequest request, Guid? organizationId, bool? isActive, CancellationToken cancellationToken);
    void Update(ProjectCategory category);
}

public interface IProjectRepository
{
    Task<bool> ExistsByCodeAsync(Guid organizationId, string code, CancellationToken cancellationToken);
    Task<bool> ExistsByCodeAsync(Guid organizationId, string code, Guid excludeId, CancellationToken cancellationToken);
    Task AddAsync(Project project, CancellationToken cancellationToken);
    Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Project?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResponse<Project>> GetPagedAsync(GetProjectsQuery request, CancellationToken cancellationToken);
    void Update(Project project);
}

public interface IDraftSessionRepository
{
    Task<DraftSession?> GetByUserAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken);
    Task AddAsync(DraftSession session, CancellationToken cancellationToken);
    void Update(DraftSession session);
    void Remove(DraftSession session);
}

public interface IProjectTeamRepository
{
    Task AddAsync(ProjectTeamMember member, CancellationToken cancellationToken);
    Task<ProjectTeamMember?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> HasActiveAsync(Guid projectId, Guid employeeId, CancellationToken cancellationToken);
    Task<PagedResponse<ProjectTeamMember>> GetPagedAsync(Guid projectId, TeamMemberStatus? status, PagedRequest request, CancellationToken cancellationToken);
    Task<List<ProjectTeamMember>> GetAllAsync(Guid projectId, CancellationToken cancellationToken);
    void Update(ProjectTeamMember member);
    void Remove(ProjectTeamMember member);
}

public interface IProjectAssetRepository
{
    Task AddAsync(ProjectAssetAllocation allocation, CancellationToken cancellationToken);
    Task<ProjectAssetAllocation?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> HasActiveAsync(Guid projectId, Guid assetId, CancellationToken cancellationToken);
    Task<PagedResponse<ProjectAssetAllocation>> GetPagedAsync(Guid projectId, AllocationStatus? status, PagedRequest request, CancellationToken cancellationToken);
    Task<ProjectAssetAllocation?> GetByAssetIdAsync(Guid assetId, CancellationToken cancellationToken);
    Task<List<ProjectAssetAllocation>> GetAllAsync(Guid projectId, CancellationToken cancellationToken);
    void Update(ProjectAssetAllocation allocation);
}

public interface IProjectDocumentRepository
{
    Task AddAsync(ProjectDocument document, CancellationToken cancellationToken);
    Task<ProjectDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResponse<ProjectDocument>> GetPagedAsync(Guid projectId, DocumentCategory? category, string? search, PagedRequest request, CancellationToken cancellationToken);
    Task<List<ProjectDocument>> GetVersionHistoryAsync(Guid projectId, string documentName, CancellationToken cancellationToken);
    Task<List<ProjectDocument>> GetAllAsync(Guid projectId, CancellationToken cancellationToken);
    void Update(ProjectDocument document);
}

public interface IProjectParameterRepository
{
    Task AddSectionAsync(ProjectParameterSection section, CancellationToken cancellationToken);
    Task<ProjectParameterSection?> GetSectionByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<ProjectParameterSection>> GetSectionsAsync(Guid projectId, CancellationToken cancellationToken);
    Task AddParameterAsync(ProjectParameter parameter, CancellationToken cancellationToken);
    Task<ProjectParameter?> GetParameterByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpsertValueAsync(ProjectParameterValue value, CancellationToken cancellationToken);
    Task<List<ProjectParameterValue>> GetValuesByProjectAsync(Guid projectId, CancellationToken cancellationToken);
    void UpdateSection(ProjectParameterSection section);
    void UpdateParameter(ProjectParameter parameter);
    void RemoveSection(ProjectParameterSection section);
    void RemoveParameter(ProjectParameter parameter);
}

public interface IProjectBudgetRepository
{
    Task AddAsync(ProjectBudget budget, CancellationToken cancellationToken);
    Task<ProjectBudget?> GetLatestByProjectAsync(Guid projectId, CancellationToken cancellationToken);
    Task<PagedResponse<ProjectBudget>> GetHistoryAsync(Guid projectId, PagedRequest request, CancellationToken cancellationToken);
}

public interface IProjectRiskRepository
{
    Task AddAsync(ProjectRisk risk, CancellationToken cancellationToken);
    Task<ProjectRisk?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResponse<ProjectRisk>> GetPagedAsync(GetRisksQuery request, CancellationToken cancellationToken);
    Task<List<ProjectRisk>> GetAllAsync(Guid projectId, CancellationToken cancellationToken);
    void Update(ProjectRisk risk);
    void Remove(ProjectRisk risk);
}

public interface IProjectTimelineRepository
{
    Task AddAsync(ProjectTimelineEvent timelineEvent, CancellationToken cancellationToken);
    Task<PagedResponse<ProjectTimelineEvent>> GetPagedAsync(Guid projectId, TimelineEventType? type, string? keyword, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<ProjectTimelineEvent>> GetAllAsync(Guid projectId, CancellationToken cancellationToken);
}

public interface IProjectActivityRepository
{
    Task AddAsync(ProjectActivityRecord activity, CancellationToken cancellationToken);
    Task<PagedResponse<ProjectActivityRecord>> GetPagedAsync(GetActivitiesQuery request, CancellationToken cancellationToken);
    Task<List<ProjectActivityRecord>> GetRecentAsync(Guid projectId, int count, CancellationToken cancellationToken);
}

public interface ISavedFilterRepository
{
    Task AddAsync(SavedFilter filter, CancellationToken cancellationToken);
    Task<SavedFilter?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<SavedFilter>> GetByUserAsync(Guid userId, Guid organizationId, string? entityType, CancellationToken cancellationToken);
    void Remove(SavedFilter filter);
}
