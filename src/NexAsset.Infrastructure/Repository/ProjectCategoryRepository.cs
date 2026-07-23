using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Repository;

public sealed class ProjectCategoryRepository : IProjectCategoryRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectCategoryRepository(ApplicationDbContext context) => _context = context;

    public Task<bool> ExistsByNameAsync(Guid organizationId, string name, CancellationToken cancellationToken) =>
        _context.ProjectCategories.AnyAsync(x => x.OrganizationId == organizationId && x.Name.ToLower() == name.ToLower() && !x.IsDeleted, cancellationToken);

    public Task<bool> ExistsByNameAsync(Guid organizationId, string name, Guid excludeId, CancellationToken cancellationToken) =>
        _context.ProjectCategories.AnyAsync(x => x.OrganizationId == organizationId && x.Name.ToLower() == name.ToLower() && x.Id != excludeId && !x.IsDeleted, cancellationToken);

    public async Task AddAsync(ProjectCategory category, CancellationToken cancellationToken) =>
        await _context.ProjectCategories.AddAsync(category, cancellationToken);

    public Task<ProjectCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _context.ProjectCategories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public async Task<PagedResponse<ProjectCategory>> GetPagedAsync(PagedRequest request, bool? isActive, CancellationToken cancellationToken)
    {
        IQueryable<ProjectCategory> query = _context.ProjectCategories.AsNoTracking().Where(x => !x.IsDeleted);

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(search) || (x.Description != null && x.Description.ToLower().Contains(search)));
        }

        query = request.SortBy?.ToLower() switch
        {
            "createdat" or "createdatutc" => request.Descending ? query.OrderByDescending(x => x.CreatedAtUtc) : query.OrderBy(x => x.CreatedAtUtc),
            _ => request.Descending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync(cancellationToken);

        return new PagedResponse<ProjectCategory>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public void Update(ProjectCategory category) => _context.ProjectCategories.Update(category);
}
