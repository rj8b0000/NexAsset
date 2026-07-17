using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NexAsset.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Holds the effective permission codes of the signed-in user, populated from
    /// <c>GET /api/auth/me/permissions</c> by the authentication flow. Checks are fail-open
    /// while unloaded because the backend independently enforces every permission — the UI
    /// only hides what the server would reject anyway.
    /// </summary>
    public sealed class PermissionChecker : IPermissionChecker
    {
        private readonly IAuthenticationApiClient _apiClient;
        private HashSet<string>? _permissions;

        public PermissionChecker(IAuthenticationApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public bool IsLoaded => _permissions is not null;
        public bool IsSuperAdmin { get; private set; }
        public Guid? OrganizationId { get; private set; }
        public string? OrganizationName { get; private set; }
        public event Action? OnChanged;

        public bool HasPermission(string permission)
        {
            if (_permissions is null) return true;
            return IsSuperAdmin || _permissions.Contains(permission);
        }

        public async Task LoadAsync(CancellationToken cancellationToken = default)
        {
            var result = await _apiClient.GetMyPermissionsAsync(cancellationToken);
            if (!result.IsSuccess || result.Data is null)
                return; // stay fail-open; the backend still enforces

            _permissions = new HashSet<string>(result.Data.Permissions, StringComparer.OrdinalIgnoreCase);
            IsSuperAdmin = result.Data.IsSuperAdmin;
            OrganizationId = result.Data.OrganizationId;
            OrganizationName = result.Data.OrganizationName;
            OnChanged?.Invoke();
        }

        public void Clear()
        {
            _permissions = null;
            IsSuperAdmin = false;
            OrganizationId = null;
            OrganizationName = null;
            OnChanged?.Invoke();
        }
    }
}
