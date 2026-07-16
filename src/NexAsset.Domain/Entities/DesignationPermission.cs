namespace NexAsset.Domain.Entities;

/// <summary>
/// Join between a designation (job title) and a permission, so employees inherit
/// capabilities from their designation. Mirrors <see cref="RolePermission"/>.
/// </summary>
public class DesignationPermission
{
    public Guid DesignationId { get; set; }
    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
