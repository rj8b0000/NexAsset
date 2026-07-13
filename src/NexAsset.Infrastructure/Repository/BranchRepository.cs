using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class BranchRepository : IBranchRepository
{
    private readonly ApplicationDbContext _context;

    public BranchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByCodeAsync(
        Guid organizationId,
        string code,
        CancellationToken cancellationToken)
    {
        return await _context.Branches
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
        return await _context.Branches
            .AnyAsync(
                x => x.OrganizationId == organizationId &&
                     x.Code == code &&
                     x.Id != excludeId &&
                     !x.IsDeleted,
                cancellationToken);
    }

    public async Task AddAsync(
        Branch branch,
        CancellationToken cancellationToken)
    {
        await _context.Branches.AddAsync(branch, cancellationToken);
    }

    public async Task<Branch?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Branches
            .FirstOrDefaultAsync(
                x => x.Id == id && !x.IsDeleted,
                cancellationToken);
    }

    public async Task<PagedResponse<Branch>> GetPagedAsync(
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        IQueryable<Branch> queryable = _context.Branches
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            queryable = queryable.Where(x =>
                x.Code.ToLower().Contains(search) ||
                x.Name.ToLower().Contains(search) ||
                (x.Email != null && x.Email.ToLower().Contains(search)) ||
                (x.City != null && x.City.ToLower().Contains(search)));
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

        return new PagedResponse<Branch>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public void Update(Branch branch)
    {
        _context.Branches.Update(branch);
    }
}
