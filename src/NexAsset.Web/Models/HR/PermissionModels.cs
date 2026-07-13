using System;

namespace NexAsset.Web.Models.HR
{
    // Wire contracts for /api/permissions. Backend permission = Code + Name + Description + IsActive
    // (no separate Module/Action fields — the brief's "Module/Action" don't exist; they live inside Code).

    public sealed record PermissionListItem(Guid Id, string Code, string Name, bool IsActive);

    public sealed record PermissionDetail(Guid Id, string Code, string Name, string? Description, bool IsActive);

    /// <summary>Create/edit form model. Required (server-validated): Code, Name.</summary>
    public sealed class PermissionFormModel
    {
        public Guid? Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public static PermissionFormModel FromDetail(PermissionDetail d) => new()
        {
            Id = d.Id, Code = d.Code, Name = d.Name, Description = d.Description, IsActive = d.IsActive
        };
    }

    /// <summary>Body for POST /api/permissions/roles/assign (role↔permission mapping).</summary>
    public sealed record AssignPermissionToRoleRequest(Guid RoleId, Guid PermissionId);
}
