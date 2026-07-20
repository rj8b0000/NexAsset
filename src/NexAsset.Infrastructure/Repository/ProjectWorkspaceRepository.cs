using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Common;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class ProjectWorkspaceRepository : IProjectWorkspaceRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectWorkspaceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
        where TEntity : BaseEntity
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public void Update<TEntity>(TEntity entity)
        where TEntity : BaseEntity
    {
        _context.Set<TEntity>().Update(entity);
    }

    public Task<TEntity?> GetByIdAsync<TEntity>(Guid id, CancellationToken cancellationToken)
        where TEntity : BaseEntity
    {
        return _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public Task<PagedResponse<ProjectCategory>> GetCategoriesAsync(PagedRequest request, CancellationToken cancellationToken)
    {
        var query = _context.ProjectCategories.AsNoTracking().Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(search) || (x.Description != null && x.Description.ToLower().Contains(search)));
        }

        query = request.SortBy?.ToLowerInvariant() switch
        {
            "name" => request.Descending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            _ => request.Descending ? query.OrderByDescending(x => x.CreatedAtUtc) : query.OrderBy(x => x.CreatedAtUtc)
        };

        return Page(query, request, cancellationToken);
    }

    public Task<PagedResponse<Project>> GetProjectsAsync(PagedRequest request, CancellationToken cancellationToken)
    {
        var query = _context.Projects.AsNoTracking().Where(x => !x.IsDeleted && !x.IsArchived);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(x => x.ProjectName.ToLower().Contains(search) || (x.Description != null && x.Description.ToLower().Contains(search)) || (x.Notes != null && x.Notes.ToLower().Contains(search)));
        }

        query = request.SortBy?.ToLowerInvariant() switch
        {
            "projectname" => request.Descending ? query.OrderByDescending(x => x.ProjectName) : query.OrderBy(x => x.ProjectName),
            "priority" => request.Descending ? query.OrderByDescending(x => x.Priority) : query.OrderBy(x => x.Priority),
            "status" => request.Descending ? query.OrderByDescending(x => x.Status) : query.OrderBy(x => x.Status),
            "startdate" => request.Descending ? query.OrderByDescending(x => x.StartDate) : query.OrderBy(x => x.StartDate),
            "expectedcompletion" => request.Descending ? query.OrderByDescending(x => x.ExpectedCompletion) : query.OrderBy(x => x.ExpectedCompletion),
            _ => request.Descending ? query.OrderByDescending(x => x.CreatedAtUtc) : query.OrderBy(x => x.CreatedAtUtc)
        };

        return Page(query, request, cancellationToken);
    }

    public Task<List<ProjectMember>> GetMembersAsync(Guid projectId, CancellationToken cancellationToken) =>
        _context.ProjectMembers.AsNoTracking().Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderBy(x => x.JoinedOn).ToListAsync(cancellationToken);

    public Task<List<ProjectAssetAllocation>> GetAssetAllocationsAsync(Guid projectId, CancellationToken cancellationToken) =>
        _context.ProjectAssetAllocations.AsNoTracking().Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderByDescending(x => x.AllocatedOn).ToListAsync(cancellationToken);

    public Task<List<ProjectAssetAllocation>> GetAssetHistoryAsync(Guid assetId, CancellationToken cancellationToken) =>
        _context.ProjectAssetAllocations.AsNoTracking().Where(x => x.AssetId == assetId && !x.IsDeleted).OrderByDescending(x => x.AllocatedOn).ToListAsync(cancellationToken);

    public Task<List<ProjectParameterGroup>> GetParameterGroupsAsync(Guid projectId, CancellationToken cancellationToken) =>
        _context.ProjectParameterGroups.AsNoTracking().Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderBy(x => x.DisplayOrder).ToListAsync(cancellationToken);

    public Task<List<ProjectParameter>> GetParametersAsync(Guid projectId, CancellationToken cancellationToken) =>
        _context.ProjectParameters.AsNoTracking().Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderBy(x => x.DisplayOrder).ToListAsync(cancellationToken);

    public Task<List<ProjectDocument>> GetDocumentsAsync(Guid projectId, CancellationToken cancellationToken) =>
        _context.ProjectDocuments.AsNoTracking().Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderByDescending(x => x.UploadedOn).ToListAsync(cancellationToken);

    public Task<List<ProjectActivity>> GetActivitiesAsync(Guid projectId, CancellationToken cancellationToken) =>
        _context.ProjectActivities.AsNoTracking().Where(x => x.ProjectId == projectId && !x.IsDeleted).OrderByDescending(x => x.OccurredAtUtc).ToListAsync(cancellationToken);

    public Task<ProjectDraft?> GetDraftAsync(Guid id, CancellationToken cancellationToken) =>
        _context.ProjectDrafts.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted && !x.IsSubmitted, cancellationToken);

    public Task<PagedResponse<ProjectDraft>> GetDraftsAsync(PagedRequest request, CancellationToken cancellationToken)
    {
        var query = _context.ProjectDrafts.AsNoTracking().Where(x => !x.IsDeleted && !x.IsSubmitted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(x => x.DraftName.ToLower().Contains(search));
        }

        query = request.Descending ? query.OrderByDescending(x => x.LastSavedAtUtc) : query.OrderBy(x => x.LastSavedAtUtc);
        return Page(query, request, cancellationToken);
    }

    private static async Task<PagedResponse<TEntity>> Page<TEntity>(
        IQueryable<TEntity> query,
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResponse<TEntity>
        {
            Items = items,
            TotalCount = total,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<ProjectBudget?> GetBudgetAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Set<ProjectBudget>().FirstOrDefaultAsync(x => x.ProjectId == projectId, cancellationToken);
    }

    public async Task<List<ProjectRisk>> GetRisksAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Set<ProjectRisk>().Where(x => x.ProjectId == projectId).ToListAsync(cancellationToken);
    }

    public async Task<List<ProjectSetting>> GetSettingsAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Set<ProjectSetting>().Where(x => x.ProjectId == projectId).ToListAsync(cancellationToken);
    }
}
