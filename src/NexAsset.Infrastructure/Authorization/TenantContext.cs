using NexAsset.Application.Common.Interfaces;

namespace NexAsset.Infrastructure.Authorization;

/// <inheritdoc cref="ITenantContext" />
public sealed class TenantContext : ITenantContext
{
    public Guid? FilterOrganizationId { get; private set; }

    public Guid? OrganizationFilterId { get; private set; }

    public bool IsSuperAdmin { get; private set; }

    public void Apply(Guid? organizationId, bool isSuperAdmin, bool isAuthenticated, Guid? selectedOrganizationId)
    {
        IsSuperAdmin = isSuperAdmin;

        if (!isAuthenticated)
        {
            // No user (startup seeding, background work): unrestricted.
            FilterOrganizationId = null;
            OrganizationFilterId = null;
            return;
        }

        if (isSuperAdmin)
        {
            // Restrict everything to the switcher selection, but never the organization list
            // itself — a SuperAdmin must always be able to see and switch between organizations.
            FilterOrganizationId = selectedOrganizationId;
            OrganizationFilterId = null;
            return;
        }

        // Everyone else is pinned to their own organization, falling back to Guid.Empty so an
        // org-less account reads nothing rather than everything.
        var own = organizationId ?? Guid.Empty;
        FilterOrganizationId = own;
        OrganizationFilterId = own;
    }
}
