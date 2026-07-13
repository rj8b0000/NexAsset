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
}
