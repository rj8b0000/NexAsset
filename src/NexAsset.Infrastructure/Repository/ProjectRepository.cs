using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Repository;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context) => _context = context;

    public Task<bool> ExistsByCodeAsync(Guid organizationId, string projectCode, CancellationToken cancellationToken) =>
        _context.Projects.AnyAsync(x => x.OrganizationId == organizationId && x.ProjectCode == projectCode && !x.IsDeleted, cancellationToken);

    public Task<bool> ExistsByCodeAsync(Guid organizationId, string projectCode, Guid excludeId, CancellationToken cancellationToken) =>
        _context.Projects.AnyAsync(x => x.OrganizationId == organizationId && x.ProjectCode == projectCode && x.Id != excludeId && !x.IsDeleted, cancellationToken);

    public async Task AddAsync(Project project, CancellationToken cancellationToken) =>
        await _context.Projects.AddAsync(project, cancellationToken);

    public Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _context.Projects.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public Task<Project?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken) =>
        _context.Projects
            .Include(x => x.Category)
            .Include(x => x.Customer)
            .Include(x => x.Branch)
            .Include(x => x.Department)
            .Include(x => x.ProjectManager)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public async Task<PagedResponse<Project>> GetPagedAsync(
        PagedRequest request,
        ProjectStatus? status,
        ProjectPriority? priority,
        Guid? categoryId,
        Guid? branchId,
        Guid? departmentId,
        Guid? projectManagerEmployeeId,
        CancellationToken cancellationToken)
    {
        IQueryable<Project> query = _context.Projects
            .Include(x => x.Category)
            .Include(x => x.ProjectManager)
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (status.HasValue) query = query.Where(x => x.Status == status.Value);
        if (priority.HasValue) query = query.Where(x => x.Priority == priority.Value);
        if (categoryId.HasValue) query = query.Where(x => x.CategoryId == categoryId.Value);
        if (branchId.HasValue) query = query.Where(x => x.BranchId == branchId.Value);
        if (departmentId.HasValue) query = query.Where(x => x.DepartmentId == departmentId.Value);
        if (projectManagerEmployeeId.HasValue) query = query.Where(x => x.ProjectManagerEmployeeId == projectManagerEmployeeId.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(x => x.ProjectCode.ToLower().Contains(search) || x.ProjectName.ToLower().Contains(search));
        }

        query = request.SortBy?.ToLower() switch
        {
            "code" or "projectcode" => request.Descending ? query.OrderByDescending(x => x.ProjectCode) : query.OrderBy(x => x.ProjectCode),
            "startdate" => request.Descending ? query.OrderByDescending(x => x.StartDate) : query.OrderBy(x => x.StartDate),
            "enddate" => request.Descending ? query.OrderByDescending(x => x.EndDate) : query.OrderBy(x => x.EndDate),
            "createdat" or "createdatutc" => request.Descending ? query.OrderByDescending(x => x.CreatedAtUtc) : query.OrderBy(x => x.CreatedAtUtc),
            _ => request.Descending ? query.OrderByDescending(x => x.ProjectName) : query.OrderBy(x => x.ProjectName)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync(cancellationToken);

        return new PagedResponse<Project>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public void Update(Project project) => _context.Projects.Update(project);
}
