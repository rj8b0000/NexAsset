using NexAsset.Application.Common.Interfaces;

namespace NexAsset.Infrastructure.Authorization;

/// <inheritdoc cref="ITenantContext" />
public sealed class TenantContext : ITenantContext
{
    public Guid? FilterOrganizationId { get; private set; }

    public bool IsSuperAdmin { get; private set; }

    public void Apply(Guid? organizationId, bool isSuperAdmin, bool isAuthenticated)
    {
        IsSuperAdmin = isSuperAdmin;

        // No user (seeding/background) or SuperAdmin: unrestricted. Otherwise pin to the user's
        // organization, falling back to Guid.Empty so an org-less account reads nothing.
        FilterOrganizationId = !isAuthenticated || isSuperAdmin
            ? null
            : organizationId ?? Guid.Empty;
    }
}
