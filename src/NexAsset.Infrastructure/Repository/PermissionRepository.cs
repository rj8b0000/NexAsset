using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class PermissionRepository : IPermissionRepository
{
    private readonly ApplicationDbContext _context;

    public PermissionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByCodeAsync(
        string code,
        CancellationToken cancellationToken)
    {
        return await _context.Permissions.AnyAsync(
            x => x.Code == code && !x.IsDeleted,
            cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(
        string code,
        Guid excludeId,
        CancellationToken cancellationToken)
    {
        return await _context.Permissions.AnyAsync(
            x => x.Code == code &&
                 x.Id != excludeId &&
                 !x.IsDeleted,
            cancellationToken);
    }

    public async Task AddAsync(
        Permission permission,
        CancellationToken cancellationToken)
    {
        await _context.Permissions.AddAsync(permission, cancellationToken);
    }

    public async Task<Permission?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Permissions
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<PagedResponse<Permission>> GetPagedAsync(
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        IQueryable<Permission> queryable = _context.Permissions
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

        return new PagedResponse<Permission>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<bool> RolePermissionExistsAsync(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken)
    {
        return await _context.RolePermissions.AnyAsync(
            x => x.RoleId == roleId && x.PermissionId == permissionId,
            cancellationToken);
    }

    public async Task AddRolePermissionAsync(
        RolePermission rolePermission,
        CancellationToken cancellationToken)
    {
        await _context.RolePermissions.AddAsync(rolePermission, cancellationToken);
    }

    public async Task<RolePermission?> GetRolePermissionAsync(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken)
    {
        return await _context.RolePermissions.FirstOrDefaultAsync(
            x => x.RoleId == roleId && x.PermissionId == permissionId,
            cancellationToken);
    }

    public async Task<List<Permission>> GetByRoleIdAsync(
        Guid roleId,
        CancellationToken cancellationToken)
    {
        return await _context.RolePermissions
            .AsNoTracking()
            .Where(x => x.RoleId == roleId && !x.Permission.IsDeleted)
            .Select(x => x.Permission)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public void Update(Permission permission)
    {
        _context.Permissions.Update(permission);
    }

    public void RemoveRolePermission(RolePermission rolePermission)
    {
        _context.RolePermissions.Remove(rolePermission);
    }
}
