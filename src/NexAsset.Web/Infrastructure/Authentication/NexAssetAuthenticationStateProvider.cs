using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using NexAsset.Web.Models.Auth;

namespace NexAsset.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Production <see cref="AuthenticationStateProvider"/> backed by the real backend.
    /// It is the single source of truth for "is anyone signed in" across the app —
    /// <c>&lt;AuthorizeView&gt;</c>, <c>[Authorize]</c>, and <c>CascadingAuthenticationState</c>
    /// all read from it.
    ///
    /// Session resolution:
    ///  • On first request (app startup / browser refresh) it calls <c>GET /api/auth/me</c>.
    ///    A valid cookie yields an authenticated principal; a 401 yields anonymous. This is
    ///    what persists the session across refreshes without any client-side token.
    ///  • The result is cached so routing and every AuthorizeView don't each re-hit the network;
    ///    <see cref="LoginAsync"/>-driven <see cref="MarkAuthenticated"/> / <see cref="MarkLoggedOut"/>
    ///    and <see cref="RefreshAsync"/> update the cache and notify the UI.
    /// </summary>
    public sealed class NexAssetAuthenticationStateProvider : AuthenticationStateProvider
    {
        private const string AuthenticationType = "NexAssetCookie";

        private static readonly AuthenticationState Anonymous =
            new(new ClaimsPrincipal(new ClaimsIdentity()));

        private readonly IAuthenticationApiClient _apiClient;
        private AuthenticationState? _cached;

        public NexAssetAuthenticationStateProvider(IAuthenticationApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        /// <summary>
        /// Synchronous view of the last-known auth status, for callers that must not await
        /// (e.g. the 401 handler deciding whether a session actually lapsed).
        /// </summary>
        public bool IsCurrentlyAuthenticated =>
            _cached?.User.Identity?.IsAuthenticated == true;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_cached is not null)
            {
                return _cached;
            }

            var result = await _apiClient.GetCurrentUserAsync();
            _cached = result.IsSuccess && result.Data is not null
                ? new AuthenticationState(BuildPrincipal(AuthenticatedUser.FromCurrentUser(result.Data)))
                : Anonymous;

            return _cached;
        }

        /// <summary>Adopt a freshly signed-in user and push the change to the UI.</summary>
        public void MarkAuthenticated(AuthenticatedUser user)
        {
            _cached = new AuthenticationState(BuildPrincipal(user));
            NotifyAuthenticationStateChanged(Task.FromResult(_cached));
        }

        /// <summary>Drop to anonymous and push the change to the UI.</summary>
        public void MarkLoggedOut()
        {
            _cached = Anonymous;
            NotifyAuthenticationStateChanged(Task.FromResult(_cached));
        }

        /// <summary>Force a re-fetch from <c>/api/auth/me</c> on the next state read.</summary>
        public void RefreshAsync()
        {
            _cached = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private static ClaimsPrincipal BuildPrincipal(AuthenticatedUser user)
        {
            var identity = new ClaimsIdentity(AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, string.IsNullOrWhiteSpace(user.DisplayName) ? user.Email : user.DisplayName));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            // Roles/permissions claims will be added here once NexAsset.API exposes them,
            // enabling role-aware AuthorizeView without any further wiring.
            return new ClaimsPrincipal(identity);
        }
    }
}
