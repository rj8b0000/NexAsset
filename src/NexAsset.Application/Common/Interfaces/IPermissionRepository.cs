using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IPermissionRepository
{
    Task<bool> ExistsByCodeAsync(
        string code,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        string code,
        Guid excludeId,
        CancellationToken cancellationToken);

    Task AddAsync(
        Permission permission,
        CancellationToken cancellationToken);

    Task<Permission?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResponse<Permission>> GetPagedAsync(
        PagedRequest request,
        CancellationToken cancellationToken);

    Task<bool> RolePermissionExistsAsync(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken);

    Task AddRolePermissionAsync(
        RolePermission rolePermission,
        CancellationToken cancellationToken);

    Task<RolePermission?> GetRolePermissionAsync(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken);

    Task<List<Permission>> GetByRoleIdAsync(
        Guid roleId,
        CancellationToken cancellationToken);

    void Update(Permission permission);

    void RemoveRolePermission(RolePermission rolePermission);

    // Designation ↔ permission mapping (mirrors the role mapping above).
    Task<bool> DesignationPermissionExistsAsync(
        Guid designationId,
        Guid permissionId,
        CancellationToken cancellationToken);

    Task AddDesignationPermissionAsync(
        DesignationPermission designationPermission,
        CancellationToken cancellationToken);

    Task<DesignationPermission?> GetDesignationPermissionAsync(
        Guid designationId,
        Guid permissionId,
        CancellationToken cancellationToken);

    Task<List<Permission>> GetByDesignationIdAsync(
        Guid designationId,
        CancellationToken cancellationToken);

    void RemoveDesignationPermission(DesignationPermission designationPermission);

    /// <summary>
    /// Drops every role and designation mapping for a permission. Called when the permission is
    /// deleted so the join rows don't linger and silently reapply if the code is ever restored.
    /// </summary>
    Task RemoveMappingsForPermissionAsync(
        Guid permissionId,
        CancellationToken cancellationToken);
}
