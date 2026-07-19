using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Authorization;

/// <summary>
/// The resolved authorization state of one user. <paramref name="OrganizationId"/>/
/// <paramref name="OrganizationName"/> describe the organization of the user's employee record
/// (null for accounts with no employee, such as the system administrator) and pin non-SuperAdmin
/// users to a single workspace. The name is included so a user can see their own workspace
/// without needing the Organizations.View permission.
/// </summary>
public sealed record EffectivePermissions(
    bool IsSuperAdmin,
    IReadOnlySet<string> Permissions,
    Guid? OrganizationId,
    string? OrganizationName);

public interface IEffectivePermissionService
{
    Task<EffectivePermissions> GetForUserAsync(Guid userId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Resolves a user's effective permission codes.
///
/// The designation is the single source of truth for employees: if the user has an employee
/// record, their permissions are exactly the ones mapped to that employee's designation — an
/// employee with no designation, or a designation with nothing mapped, has no access at all.
/// Identity roles only grant permissions to accounts that have no employee record (service or
/// administrator logins). SuperAdmin bypasses every check regardless, so the system can always
/// be recovered.
///
/// Results are cached briefly so permission checks don't hit the database on every request;
/// permission changes therefore take effect within the cache window.
/// </summary>
public sealed class EffectivePermissionService : IEffectivePermissionService
{
    private const string SuperAdminRole = "SuperAdmin";
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(60);

    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _cache;

    public EffectivePermissionService(ApplicationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<EffectivePermissions> GetForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"effective-permissions:{userId}";
        if (_cache.TryGetValue(cacheKey, out EffectivePermissions? cached) && cached is not null)
            return cached;

        var roles = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { r.Id, r.Name })
            .ToListAsync(cancellationToken);

        var isSuperAdmin = roles.Any(r => string.Equals(r.Name, SuperAdminRole, StringComparison.OrdinalIgnoreCase));

        var account = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new { u.OrganizationId })
            .FirstOrDefaultAsync(cancellationToken);

        // IgnoreQueryFilters: this runs while the organization boundary is still being decided,
        // and the boundary is derived from this very lookup — filtering here would be circular.
        var employee = await _context.Employees
            .IgnoreQueryFilters()
            .Where(e => e.IdentityUserId == userId && !e.IsDeleted)
            .Select(e => new { e.DesignationId, e.OrganizationId, OrganizationName = e.Organization.Name })
            .FirstOrDefaultAsync(cancellationToken);

        List<string> codes;
        if (employee is not null)
        {
            // Employee: the designation defines access on its own, and nothing else does.
            codes = employee.DesignationId is null
                ? new List<string>()
                : await _context.Set<DesignationPermission>()
                    .Where(dp => dp.DesignationId == employee.DesignationId
                                 && !dp.Permission.IsDeleted
                                 && dp.Permission.IsActive)
                    .Select(dp => dp.Permission.Code)
                    .ToListAsync(cancellationToken);
        }
        else
        {
            // No employee record (administrator/service account): fall back to identity roles.
            var roleIds = roles.Select(r => r.Id).ToList();
            codes = await _context.Set<RolePermission>()
                .Where(rp => roleIds.Contains(rp.RoleId) && !rp.Permission.IsDeleted && rp.Permission.IsActive)
                .Select(rp => rp.Permission.Code)
                .ToListAsync(cancellationToken);
        }

        // An employee belongs to their record's organization; a standalone account (an
        // organization administrator, say) carries its organization on the login itself.
        var organizationId = employee?.OrganizationId ?? account?.OrganizationId;
        var organizationName = employee?.OrganizationName;

        if (organizationName is null && organizationId is { } id)
        {
            organizationName = await _context.Organizations
                .IgnoreQueryFilters()
                .Where(o => o.Id == id && !o.IsDeleted)
                .Select(o => o.Name)
                .FirstOrDefaultAsync(cancellationToken);
        }

        var effective = new EffectivePermissions(
            isSuperAdmin,
            codes.ToHashSet(StringComparer.OrdinalIgnoreCase),
            organizationId,
            organizationName);

        _cache.Set(cacheKey, effective, CacheTtl);
        return effective;
    }
}
