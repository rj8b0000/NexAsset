using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Auth;

namespace NexAsset.Web.Infrastructure.Authentication
{
    /// <summary>
    /// The one entry point UI components use for authentication actions. Orchestrates the
    /// HTTP call (<see cref="IAuthenticationApiClient"/>), the auth-state change
    /// (<see cref="NexAssetAuthenticationStateProvider"/>), user notifications, and the global
    /// loading signal — so pages never touch those pieces directly and auth logic isn't
    /// duplicated across the login page, topbar, and sidebar.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Attempts a sign-in. On success the auth state flips to authenticated and a success
        /// toast is shown. On failure the returned result carries a user-safe message for the
        /// caller to render inline (no toast, to avoid duplicate error surfaces on the form).
        /// </summary>
        Task<ApiResult<AuthenticatedUser>> LoginAsync(string email, string password, bool rememberMe, CancellationToken cancellationToken = default);

        /// <summary>Signs out: clears the server cookie (best effort), drops local state, notifies.</summary>
        Task LogoutAsync(CancellationToken cancellationToken = default);
    }
}
