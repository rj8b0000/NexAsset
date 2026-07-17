using System;
using System.Threading;
using System.Threading.Tasks;

namespace NexAsset.Web.Infrastructure.Authentication
{
    /// <summary>
    /// The signed-in user's effective permission set, loaded from
    /// <c>GET /api/auth/me/permissions</c> and consulted by the UI to gate navigation and
    /// action buttons. This is cosmetic gating only — the backend enforces every permission
    /// again per request, so hiding a button here is UX, not security.
    /// </summary>
    public interface IPermissionChecker
    {
        /// <summary>True once a permission set has been loaded for the current session.</summary>
        bool IsLoaded { get; }

        /// <summary>SuperAdmin bypasses all permission checks and is the only role allowed to switch organization.</summary>
        bool IsSuperAdmin { get; }

        /// <summary>
        /// The organization the signed-in user's employee record belongs to, or null for accounts
        /// with no employee record (the system administrator). Non-SuperAdmin users are pinned to it.
        /// </summary>
        Guid? OrganizationId { get; }

        /// <summary>Display name of <see cref="OrganizationId"/>, available without Organizations.View.</summary>
        string? OrganizationName { get; }

        /// <summary>
        /// Whether the user holds the given "Module.Action" permission. Returns true while the
        /// set is not yet loaded (fail-open UI — the backend still rejects unauthorized calls).
        /// </summary>
        bool HasPermission(string permission);

        /// <summary>Fetch the effective permission set for the signed-in user.</summary>
        Task LoadAsync(CancellationToken cancellationToken = default);

        /// <summary>Drop the loaded set (on logout).</summary>
        void Clear();

        /// <summary>Raised when the loaded set changes, so nav/pages can re-render.</summary>
        event Action? OnChanged;
    }
}
