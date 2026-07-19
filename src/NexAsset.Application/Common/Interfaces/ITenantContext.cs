namespace NexAsset.Application.Common.Interfaces;

/// <summary>
/// The organization boundary for the current request. Every organization-scoped entity is
/// filtered to <see cref="FilterOrganizationId"/> at the database level, so a signed-in user
/// only ever reads their own organization's data.
/// </summary>
public interface ITenantContext
{
    /// <summary>
    /// Organization to restrict every query to, or null for unrestricted access.
    /// Null means SuperAdmin, or no user at all (startup seeding, background work).
    /// </summary>
    Guid? FilterOrganizationId { get; }

    bool IsSuperAdmin { get; }

    /// <summary>
    /// Applies the signed-in user's boundary. An authenticated non-SuperAdmin with no
    /// organization is scoped to <see cref="Guid.Empty"/> — they see nothing rather than
    /// everything.
    /// </summary>
    void Apply(Guid? organizationId, bool isSuperAdmin, bool isAuthenticated);
}
