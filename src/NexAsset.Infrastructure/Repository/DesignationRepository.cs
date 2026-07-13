using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class DesignationRepository : IDesignationRepository
{
    private readonly ApplicationDbContext _context;

    public DesignationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByTitleAsync(
        Guid organizationId,
        string title,
        CancellationToken cancellationToken)
    {
        return await _context.Designations
            .AnyAsync(
                x => x.OrganizationId == organizationId &&
                     x.Title == title &&
                     !x.IsDeleted,
                cancellationToken);
    }

    public async Task<bool> ExistsByTitleAsync(
        Guid organizationId,
        string title,
        Guid excludeId,
        CancellationToken cancellationToken)
    {
        return await _context.Designations
            .AnyAsync(
                x => x.OrganizationId == organizationId &&
                     x.Title == title &&
                     x.Id != excludeId &&
                     !x.IsDeleted,
                cancellationToken);
    }

    public async Task AddAsync(
        Designation designation,
        CancellationToken cancellationToken)
    {
        await _context.Designations.AddAsync(designation, cancellationToken);
    }

    public async Task<Designation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Designations
            .FirstOrDefaultAsync(
                x => x.Id == id && !x.IsDeleted,
                cancellationToken);
    }

    public async Task<PagedResponse<Designation>> GetPagedAsync(
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        IQueryable<Designation> queryable = _context.Designations
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            queryable = queryable.Where(x =>
                x.Title.ToLower().Contains(search) ||
                (x.Description != null &&
                 x.Description.ToLower().Contains(search)));
        }

        queryable = request.SortBy?.ToLower() switch
        {
            "createdat" or "createdatutc" => request.Descending
                ? queryable.OrderByDescending(x => x.CreatedAtUtc)
                : queryable.OrderBy(x => x.CreatedAtUtc),
            _ => request.Descending
                ? queryable.OrderByDescending(x => x.Title)
                : queryable.OrderBy(x => x.Title)
        };

        var totalCount = await queryable.CountAsync(cancellationToken);

        var items = await queryable
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResponse<Designation>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public void Update(Designation designation)
    {
        _context.Designations.Update(designation);
    }
}
