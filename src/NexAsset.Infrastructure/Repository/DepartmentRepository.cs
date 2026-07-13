using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByCodeAsync(
        Guid organizationId,
        string code,
        CancellationToken cancellationToken)
    {
        return await _context.Departments
            .AnyAsync(
                x => x.OrganizationId == organizationId &&
                     x.Code == code &&
                     !x.IsDeleted,
                cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(
        Guid organizationId,
        string code,
        Guid excludeId,
        CancellationToken cancellationToken)
    {
        return await _context.Departments
            .AnyAsync(
                x => x.OrganizationId == organizationId &&
                     x.Code == code &&
                     x.Id != excludeId &&
                     !x.IsDeleted,
                cancellationToken);
    }

    public async Task AddAsync(
        Department department,
        CancellationToken cancellationToken)
    {
        await _context.Departments.AddAsync(department, cancellationToken);
    }

    public async Task<Department?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Departments
            .FirstOrDefaultAsync(
                x => x.Id == id && !x.IsDeleted,
                cancellationToken);
    }

    public async Task<PagedResponse<Department>> GetPagedAsync(
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        IQueryable<Department> queryable = _context.Departments
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            queryable = queryable.Where(x =>
                x.Code.ToLower().Contains(search) ||
                x.Name.ToLower().Contains(search) ||
                (x.Description != null &&
                 x.Description.ToLower().Contains(search)));
        }

        queryable = request.SortBy?.ToLower() switch
        {
            "code" => request.Descending
                ? queryable.OrderByDescending(x => x.Code)
                : queryable.OrderBy(x => x.Code),
            "createdat" or "createdatutc" => request.Descending
                ? queryable.OrderByDescending(x => x.CreatedAtUtc)
                : queryable.OrderBy(x => x.CreatedAtUtc),
            _ => request.Descending
                ? queryable.OrderByDescending(x => x.Name)
                : queryable.OrderBy(x => x.Name)
        };

        var totalCount = await queryable.CountAsync(cancellationToken);

        var items = await queryable
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResponse<Department>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public void Update(Department department)
    {
        _context.Departments.Update(department);
    }
}
