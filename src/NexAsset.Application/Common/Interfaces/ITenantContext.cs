namespace NexAsset.Application.Common.Interfaces;

/// <summary>
/// The organization boundary for the current request. Every organization-scoped entity is
/// filtered to <see cref="FilterOrganizationId"/> at the database level, so a signed-in user
/// only ever reads their own organization's data.
/// </summary>
public interface ITenantContext
{
    /// <summary>
    /// Organization every organization-scoped entity is restricted to, or null for unrestricted
    /// access. For a non-SuperAdmin this is their own organization (the hard boundary); for a
    /// SuperAdmin it is the organization currently selected in the workspace switcher, or null
    /// when none is selected (they see everything).
    /// </summary>
    Guid? FilterOrganizationId { get; }

    /// <summary>
    /// Scope for the <c>Organization</c> entity itself, which is deliberately exempt from a
    /// SuperAdmin's switcher selection: a SuperAdmin must always see every organization (null)
    /// so the switcher and Organizations page keep working. A non-SuperAdmin is still pinned to
    /// their own organization.
    /// </summary>
    Guid? OrganizationFilterId { get; }

    bool IsSuperAdmin { get; }

    /// <summary>
    /// Applies the signed-in user's boundary. An authenticated non-SuperAdmin with no
    /// organization is scoped to <see cref="Guid.Empty"/> — they see nothing rather than
    /// everything. <paramref name="selectedOrganizationId"/> is the SuperAdmin's switcher
    /// selection and is ignored for everyone else.
    /// </summary>
    void Apply(Guid? organizationId, bool isSuperAdmin, bool isAuthenticated, Guid? selectedOrganizationId);
}
