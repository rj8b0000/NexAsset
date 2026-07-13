namespace NexAsset.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Stub implementation: every permission check succeeds once the mock admin user
    /// is signed in, matching today's actual behavior where any authenticated session
    /// sees every page. Replace with a real role/claim-based check once NexAsset.API's
    /// Permissions/Roles endpoints are integrated.
    /// </summary>
    public class PermissionChecker : IPermissionChecker
    {
        private readonly ICurrentUserService _currentUser;

        public PermissionChecker(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        public bool HasPermission(string permission) => _currentUser.IsAuthenticated;
    }
}
