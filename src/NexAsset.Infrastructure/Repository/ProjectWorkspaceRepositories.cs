using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Features.Projects;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class ProjectCategoryRepository(ApplicationDbContext context) : IProjectCategoryRepository
{
    public Task<bool> ExistsByNameAsync(Guid organizationId, string name, CancellationToken cancellationToken) =>
        context.ProjectCategories.AnyAsync(x => x.OrganizationId == organizationId && x.Name == name && !x.IsDeleted, cancellationToken);

    public Task<bool> ExistsByNameAsync(Guid organizationId, string name, Guid excludeId, CancellationToken cancellationToken) =>
        context.ProjectCategories.AnyAsync(x => x.OrganizationId == organizationId && x.Name == name && x.Id != excludeId && !x.IsDeleted, cancellationToken);

    public Task AddAsync(ProjectCategory category, CancellationToken cancellationToken) => context.ProjectCategories.AddAsync(category, cancellationToken).AsTask();

    public Task<ProjectCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.ProjectCategories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public async Task<PagedResponse<ProjectCategory>> GetPagedAsync(PagedRequest request, Guid? organizationId, bool? isActive, CancellationToken cancellationToken)
    {
        var query = context.ProjectCategories.AsNoTracking().Where(x => !x.IsDeleted);
        if (organizationId.HasValue) query = query.Where(x => x.OrganizationId == organizationId.Value);
        if (isActive.HasValue) query = query.Where(x => x.IsActive == isActive.Value);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(search) || (x.Description != null && x.Description.ToLower().Contains(search)));
        }

        query = request.SortBy?.ToLowerInvariant() switch
        {
            "createdat" or "createdatutc" => request.Descending ? query.OrderByDescending(x => x.CreatedAtUtc) : query.OrderBy(x => x.CreatedAtUtc),
            "isactive" => request.Descending ? query.OrderByDescending(x => x.IsActive) : query.OrderBy(x => x.IsActive),
            _ => request.Descending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name)
        };
        return await ProjectWorkspacePaging.PageAsync(query, request, cancellationToken);
    }

    public void Update(ProjectCategory category) => context.ProjectCategories.Update(category);
}

public sealed class ProjectRepository(ApplicationDbContext context) : IProjectRepository
{
    public Task<bool> ExistsByCodeAsync(Guid organizationId, string code, CancellationToken cancellationToken) =>
        context.Projects.AnyAsync(x => x.OrganizationId == organizationId && x.ProjectCode == code && !x.IsDeleted, cancellationToken);

    public Task<bool> ExistsByCodeAsync(Guid organizationId, string code, Guid excludeId, CancellationToken cancellationToken) =>
        context.Projects.AnyAsync(x => x.OrganizationId == organizationId && x.ProjectCode == code && x.Id != excludeId && !x.IsDeleted, cancellationToken);

    public Task AddAsync(Project project, CancellationToken cancellationToken) => context.Projects.AddAsync(project, cancellationToken).AsTask();

    public Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Projects.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public Task<Project?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken) =>
        Details(context.Projects).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public async Task<PagedResponse<Project>> GetPagedAsync(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var query = Details(context.Projects).AsNoTracking().Where(x => !x.IsDeleted);
        if (request.OrganizationId.HasValue) query = query.Where(x => x.OrganizationId == request.OrganizationId.Value);
        if (request.Status.HasValue) query = query.Where(x => x.Status == request.Status.Value);
        if (request.Priority.HasValue) query = query.Where(x => x.Priority == request.Priority.Value);
        if (request.CategoryId.HasValue) query = query.Where(x => x.CategoryId == request.CategoryId.Value);
        if (request.BranchId.HasValue) query = query.Where(x => x.BranchId == request.BranchId.Value);
        if (request.DepartmentId.HasValue) query = query.Where(x => x.DepartmentId == request.DepartmentId.Value);
        if (request.ProjectManagerEmployeeId.HasValue) query = query.Where(x => x.ProjectManagerEmployeeId == request.ProjectManagerEmployeeId.Value);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(x => x.ProjectCode.ToLower().Contains(search) || x.ProjectName.ToLower().Contains(search));
        }

        query = request.SortBy?.ToLowerInvariant() switch
        {
            "startdate" => request.Descending ? query.OrderByDescending(x => x.StartDate) : query.OrderBy(x => x.StartDate),
            "enddate" => request.Descending ? query.OrderByDescending(x => x.EndDate) : query.OrderBy(x => x.EndDate),
            "createdat" or "createdatutc" => request.Descending ? query.OrderByDescending(x => x.CreatedAtUtc) : query.OrderBy(x => x.CreatedAtUtc),
            _ => request.Descending ? query.OrderByDescending(x => x.ProjectName) : query.OrderBy(x => x.ProjectName)
        };
        return await ProjectWorkspacePaging.PageAsync(query, request, cancellationToken);
    }

    public void Update(Project project) => context.Projects.Update(project);

    private static IQueryable<Project> Details(IQueryable<Project> query) => query
        .Include(x => x.Category)
        .Include(x => x.Branch)
        .Include(x => x.Department)
        .Include(x => x.ProjectManager)
        .Include(x => x.Customer);
}

public sealed class DraftSessionRepository(ApplicationDbContext context) : IDraftSessionRepository
{
    public Task<DraftSession?> GetByUserAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken) =>
        context.DraftSessions.FirstOrDefaultAsync(x => x.UserId == userId && x.OrganizationId == organizationId && !x.IsDeleted, cancellationToken);

    public Task AddAsync(DraftSession session, CancellationToken cancellationToken) => context.DraftSessions.AddAsync(session, cancellationToken).AsTask();
    public void Update(DraftSession session) => context.DraftSessions.Update(session);
    public void Remove(DraftSession session) => context.DraftSessions.Remove(session);
}

public sealed class ProjectTeamRepository(ApplicationDbContext context) : IProjectTeamRepository
{
    public Task AddAsync(ProjectTeamMember member, CancellationToken cancellationToken) => context.ProjectTeamMembers.AddAsync(member, cancellationToken).AsTask();

    public Task<ProjectTeamMember?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.ProjectTeamMembers.Include(x => x.Employee).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public Task<bool> HasActiveAsync(Guid projectId, Guid employeeId, CancellationToken cancellationToken) =>
        context.ProjectTeamMembers.AnyAsync(x => x.ProjectId == projectId && x.EmployeeId == employeeId && x.Status == TeamMemberStatus.Active && !x.IsDeleted, cancellationToken);

    public async Task<PagedResponse<ProjectTeamMember>> GetPagedAsync(Guid projectId, TeamMemberStatus? status, PagedRequest request, CancellationToken cancellationToken)
    {
        var query = context.ProjectTeamMembers.AsNoTracking().Include(x => x.Employee).Where(x => x.ProjectId == projectId && !x.IsDeleted);
        if (status.HasValue) query = query.Where(x => x.Status == status.Value);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(x => x.ProjectRole.ToLower().Contains(search) || x.Employee.FirstName.ToLower().Contains(search) || x.Employee.LastName.ToLower().Contains(search) || x.Employee.EmployeeCode.ToLower().Contains(search));
        }
        query = request.SortBy?.ToLowerInvariant() switch
        {
            "joineddate" => request.Descending ? query.OrderByDescending(x => x.JoinedDate) : query.OrderBy(x => x.JoinedDate),
            "allocationpercentage" => request.Descending ? query.OrderByDescending(x => x.AllocationPercentage) : query.OrderBy(x => x.AllocationPercentage),
            _ => request.Descending ? query.OrderByDescending(x => x.CreatedAtUtc) : query.OrderBy(x => x.CreatedAtUtc)
        };
        return await ProjectWorkspacePaging.PageAsync(query, request, cancellationToken);
    }

    public Task<List<ProjectTeamMember>> GetAllAsync(Guid projectId, CancellationToken cancellationToken) =>
        context.ProjectTeamMembers.AsNoTracking().Include(x => x.Employee).Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderBy(x => x.JoinedDate).ToListAsync(cancellationToken);

    public void Update(ProjectTeamMember member) => context.ProjectTeamMembers.Update(member);
    public void Remove(ProjectTeamMember member) => context.ProjectTeamMembers.Update(member);
}

public sealed class ProjectAssetRepository(ApplicationDbContext context) : IProjectAssetRepository
{
    public Task AddAsync(ProjectAssetAllocation allocation, CancellationToken cancellationToken) => context.ProjectAssetAllocations.AddAsync(allocation, cancellationToken).AsTask();

    public Task<ProjectAssetAllocation?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.ProjectAssetAllocations.Include(x => x.Asset).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public Task<bool> HasActiveAsync(Guid projectId, Guid assetId, CancellationToken cancellationToken) =>
        context.ProjectAssetAllocations.AnyAsync(x => x.ProjectId == projectId && x.AssetId == assetId && x.Status != AllocationStatus.Returned && !x.IsDeleted, cancellationToken);

    public async Task<PagedResponse<ProjectAssetAllocation>> GetPagedAsync(Guid projectId, AllocationStatus? status, PagedRequest request, CancellationToken cancellationToken)
    {
        var query = context.ProjectAssetAllocations.AsNoTracking().Include(x => x.Asset).Where(x => x.ProjectId == projectId && !x.IsDeleted);
        if (status.HasValue) query = query.Where(x => x.Status == status.Value);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(x => x.Asset.AssetCode.ToLower().Contains(search) || x.Asset.AssetName.ToLower().Contains(search));
        }
        query = request.Descending ? query.OrderByDescending(x => x.AllocationDate) : query.OrderBy(x => x.AllocationDate);
        return await ProjectWorkspacePaging.PageAsync(query, request, cancellationToken);
    }

    public Task<ProjectAssetAllocation?> GetByAssetIdAsync(Guid assetId, CancellationToken cancellationToken) =>
        context.ProjectAssetAllocations.FirstOrDefaultAsync(x => x.AssetId == assetId && x.Status != AllocationStatus.Returned && !x.IsDeleted, cancellationToken);

    public Task<List<ProjectAssetAllocation>> GetAllAsync(Guid projectId, CancellationToken cancellationToken) =>
        context.ProjectAssetAllocations.AsNoTracking().Include(x => x.Asset).Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderByDescending(x => x.AllocationDate).ToListAsync(cancellationToken);

    public void Update(ProjectAssetAllocation allocation) => context.ProjectAssetAllocations.Update(allocation);
}

public sealed class ProjectDocumentRepository(ApplicationDbContext context) : IProjectDocumentRepository
{
    public Task AddAsync(ProjectDocument document, CancellationToken cancellationToken) => context.ProjectDocuments.AddAsync(document, cancellationToken).AsTask();

    public Task<ProjectDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.ProjectDocuments.Include(x => x.UploadedByEmployee).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public async Task<PagedResponse<ProjectDocument>> GetPagedAsync(Guid projectId, DocumentCategory? category, string? search, PagedRequest request, CancellationToken cancellationToken)
    {
        var query = context.ProjectDocuments.AsNoTracking().Include(x => x.UploadedByEmployee).Where(x => x.ProjectId == projectId && !x.IsDeleted);
        if (category.HasValue) query = query.Where(x => x.Category == category.Value);
        if (!string.IsNullOrWhiteSpace(search))
        {
            var keyword = search.ToLower();
            query = query.Where(x => x.DocumentName.ToLower().Contains(keyword) || (x.Description != null && x.Description.ToLower().Contains(keyword)) || (x.Remarks != null && x.Remarks.ToLower().Contains(keyword)));
        }
        query = request.SortBy?.ToLowerInvariant() switch
        {
            "documentname" or "name" => request.Descending ? query.OrderByDescending(x => x.DocumentName) : query.OrderBy(x => x.DocumentName),
            "version" => request.Descending ? query.OrderByDescending(x => x.Version) : query.OrderBy(x => x.Version),
            _ => request.Descending ? query.OrderByDescending(x => x.UploadedAtUtc) : query.OrderBy(x => x.UploadedAtUtc)
        };
        return await ProjectWorkspacePaging.PageAsync(query, request, cancellationToken);
    }

    public Task<List<ProjectDocument>> GetVersionHistoryAsync(Guid projectId, string documentName, CancellationToken cancellationToken) =>
        context.ProjectDocuments.AsNoTracking().Include(x => x.UploadedByEmployee).Where(x => x.ProjectId == projectId && x.DocumentName == documentName && !x.IsDeleted).OrderByDescending(x => x.Version).ToListAsync(cancellationToken);

    public Task<List<ProjectDocument>> GetAllAsync(Guid projectId, CancellationToken cancellationToken) =>
        context.ProjectDocuments.AsNoTracking().Include(x => x.UploadedByEmployee).Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderByDescending(x => x.UploadedAtUtc).ToListAsync(cancellationToken);

    public void Update(ProjectDocument document) => context.ProjectDocuments.Update(document);
}

public sealed class ProjectParameterRepository(ApplicationDbContext context) : IProjectParameterRepository
{
    public Task AddSectionAsync(ProjectParameterSection section, CancellationToken cancellationToken) => context.ProjectParameterSections.AddAsync(section, cancellationToken).AsTask();

    public Task<ProjectParameterSection?> GetSectionByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.ProjectParameterSections.Include(x => x.Parameters).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public Task<List<ProjectParameterSection>> GetSectionsAsync(Guid projectId, CancellationToken cancellationToken) =>
        context.ProjectParameterSections.AsNoTracking().Include(x => x.Parameters.Where(p => !p.IsDeleted)).Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderBy(x => x.DisplayOrder).ToListAsync(cancellationToken);

    public Task AddParameterAsync(ProjectParameter parameter, CancellationToken cancellationToken) => context.ProjectParameters.AddAsync(parameter, cancellationToken).AsTask();

    public Task<ProjectParameter?> GetParameterByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.ProjectParameters.Include(x => x.Section).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public async Task UpsertValueAsync(ProjectParameterValue value, CancellationToken cancellationToken)
    {
        var existing = await context.ProjectParameterValues.FirstOrDefaultAsync(x => x.ProjectId == value.ProjectId && x.ParameterId == value.ParameterId && !x.IsDeleted, cancellationToken);
        if (existing is null)
            await context.ProjectParameterValues.AddAsync(value, cancellationToken);
        else
        {
            existing.Value = value.Value;
            existing.UpdatedAtUtc = DateTime.UtcNow;
        }
    }

    public Task<List<ProjectParameterValue>> GetValuesByProjectAsync(Guid projectId, CancellationToken cancellationToken) =>
        context.ProjectParameterValues.AsNoTracking().Where(x => x.ProjectId == projectId && !x.IsDeleted).ToListAsync(cancellationToken);

    public void UpdateSection(ProjectParameterSection section) => context.ProjectParameterSections.Update(section);
    public void UpdateParameter(ProjectParameter parameter) => context.ProjectParameters.Update(parameter);
    public void RemoveSection(ProjectParameterSection section) => context.ProjectParameterSections.Update(section);
    public void RemoveParameter(ProjectParameter parameter) => context.ProjectParameters.Update(parameter);
}

public sealed class ProjectBudgetRepository(ApplicationDbContext context) : IProjectBudgetRepository
{
    public Task AddAsync(ProjectBudget budget, CancellationToken cancellationToken) => context.ProjectBudgets.AddAsync(budget, cancellationToken).AsTask();

    public Task<ProjectBudget?> GetLatestByProjectAsync(Guid projectId, CancellationToken cancellationToken) =>
        context.ProjectBudgets.AsNoTracking().Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderByDescending(x => x.CreatedAtUtc).FirstOrDefaultAsync(cancellationToken);

    public async Task<PagedResponse<ProjectBudget>> GetHistoryAsync(Guid projectId, PagedRequest request, CancellationToken cancellationToken)
    {
        var query = context.ProjectBudgets.AsNoTracking().Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderByDescending(x => x.CreatedAtUtc);
        return await ProjectWorkspacePaging.PageAsync(query, request, cancellationToken);
    }
}

public sealed class ProjectRiskRepository(ApplicationDbContext context) : IProjectRiskRepository
{
    public Task AddAsync(ProjectRisk risk, CancellationToken cancellationToken) => context.ProjectRisks.AddAsync(risk, cancellationToken).AsTask();

    public Task<ProjectRisk?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.ProjectRisks.Include(x => x.OwnerEmployee).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public async Task<PagedResponse<ProjectRisk>> GetPagedAsync(GetRisksQuery request, CancellationToken cancellationToken)
    {
        var query = context.ProjectRisks.AsNoTracking().Include(x => x.OwnerEmployee).Where(x => x.ProjectId == request.ProjectId && !x.IsDeleted);
        if (request.Category.HasValue) query = query.Where(x => x.Category == request.Category.Value);
        if (request.Probability.HasValue) query = query.Where(x => x.Probability == request.Probability.Value);
        if (request.Impact.HasValue) query = query.Where(x => x.Impact == request.Impact.Value);
        if (request.Severity.HasValue) query = query.Where(x => x.Severity == request.Severity.Value);
        if (request.Status.HasValue) query = query.Where(x => x.Status == request.Status.Value);
        if (request.OwnerEmployeeId.HasValue) query = query.Where(x => x.OwnerEmployeeId == request.OwnerEmployeeId.Value);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(x => x.Title.ToLower().Contains(search) || (x.Description != null && x.Description.ToLower().Contains(search)) || (x.MitigationPlan != null && x.MitigationPlan.ToLower().Contains(search)));
        }
        query = request.SortBy?.ToLowerInvariant() switch
        {
            "severity" => request.Descending ? query.OrderByDescending(x => x.Severity) : query.OrderBy(x => x.Severity),
            "status" => request.Descending ? query.OrderByDescending(x => x.Status) : query.OrderBy(x => x.Status),
            _ => request.Descending ? query.OrderByDescending(x => x.CreatedAtUtc) : query.OrderBy(x => x.CreatedAtUtc)
        };
        return await ProjectWorkspacePaging.PageAsync(query, request, cancellationToken);
    }

    public Task<List<ProjectRisk>> GetAllAsync(Guid projectId, CancellationToken cancellationToken) =>
        context.ProjectRisks.AsNoTracking().Include(x => x.OwnerEmployee).Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderByDescending(x => x.CreatedAtUtc).ToListAsync(cancellationToken);

    public void Update(ProjectRisk risk) => context.ProjectRisks.Update(risk);
    public void Remove(ProjectRisk risk) => context.ProjectRisks.Update(risk);
}

public sealed class ProjectTimelineRepository(ApplicationDbContext context) : IProjectTimelineRepository
{
    public Task AddAsync(ProjectTimelineEvent timelineEvent, CancellationToken cancellationToken) => context.ProjectTimelineEvents.AddAsync(timelineEvent, cancellationToken).AsTask();

    public async Task<PagedResponse<ProjectTimelineEvent>> GetPagedAsync(Guid projectId, TimelineEventType? type, string? keyword, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = context.ProjectTimelineEvents.AsNoTracking().Where(x => x.ProjectId == projectId && !x.IsDeleted);
        if (type.HasValue) query = query.Where(x => x.EventType == type.Value);
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var search = keyword.ToLower();
            query = query.Where(x => x.Description.ToLower().Contains(search));
        }
        query = query.OrderByDescending(x => x.Timestamp);
        return await ProjectWorkspacePaging.PageAsync(query, new PagedRequest { PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
    }

    public Task<List<ProjectTimelineEvent>> GetAllAsync(Guid projectId, CancellationToken cancellationToken) =>
        context.ProjectTimelineEvents.AsNoTracking().Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderByDescending(x => x.Timestamp).ToListAsync(cancellationToken);
}

public sealed class ProjectActivityRepository(ApplicationDbContext context) : IProjectActivityRepository
{
    public Task AddAsync(ProjectActivityRecord activity, CancellationToken cancellationToken) => context.ProjectActivityRecords.AddAsync(activity, cancellationToken).AsTask();

    public async Task<PagedResponse<ProjectActivityRecord>> GetPagedAsync(GetActivitiesQuery request, CancellationToken cancellationToken)
    {
        var query = context.ProjectActivityRecords.AsNoTracking().Where(x => x.ProjectId == request.ProjectId && !x.IsDeleted);
        if (request.ActivityType.HasValue) query = query.Where(x => x.ActivityType == request.ActivityType.Value);
        if (request.UserId.HasValue) query = query.Where(x => x.UserId == request.UserId.Value);
        if (request.From.HasValue) query = query.Where(x => x.Timestamp >= request.From.Value);
        if (request.To.HasValue) query = query.Where(x => x.Timestamp <= request.To.Value);
        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keyword = request.Keyword.ToLower();
            query = query.Where(x => x.Action.ToLower().Contains(keyword) || (x.Remarks != null && x.Remarks.ToLower().Contains(keyword)));
        }
        query = query.OrderByDescending(x => x.Timestamp);
        return await ProjectWorkspacePaging.PageAsync(query, request, cancellationToken);
    }

    public Task<List<ProjectActivityRecord>> GetRecentAsync(Guid projectId, int count, CancellationToken cancellationToken) =>
        context.ProjectActivityRecords.AsNoTracking().Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderByDescending(x => x.Timestamp).Take(count).ToListAsync(cancellationToken);
}

public sealed class SavedFilterRepository(ApplicationDbContext context) : ISavedFilterRepository
{
    public Task AddAsync(SavedFilter filter, CancellationToken cancellationToken) => context.SavedFilters.AddAsync(filter, cancellationToken).AsTask();

    public Task<SavedFilter?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.SavedFilters.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public Task<List<SavedFilter>> GetByUserAsync(Guid userId, Guid organizationId, string? entityType, CancellationToken cancellationToken)
    {
        var query = context.SavedFilters.AsNoTracking().Where(x => x.UserId == userId && x.OrganizationId == organizationId && !x.IsDeleted);
        if (!string.IsNullOrWhiteSpace(entityType)) query = query.Where(x => x.EntityType == entityType);
        return query.OrderBy(x => x.FilterName).ToListAsync(cancellationToken);
    }

    public void Remove(SavedFilter filter) => context.SavedFilters.Remove(filter);
}

internal static class ProjectWorkspacePaging
{
    public static async Task<PagedResponse<T>> PageAsync<T>(IQueryable<T> query, PagedRequest request, CancellationToken cancellationToken)
    {
        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new PagedResponse<T> { Items = items, TotalCount = totalCount, PageNumber = pageNumber, PageSize = pageSize };
    }
}
