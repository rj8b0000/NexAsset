using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Auth;

namespace NexAsset.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Thin, transport-only contract over NexAsset.API's <c>/api/auth</c> endpoints.
    /// Returns <see cref="ApiResult{T}"/> so callers never see raw HTTP; higher-level
    /// orchestration (login/logout side effects, notifications, state changes) lives in
    /// <see cref="IAuthenticationService"/>, not here.
    /// </summary>
    public interface IAuthenticationApiClient
    {
        /// <summary>POST /api/auth/login — issues the auth cookie on success.</summary>
        Task<ApiResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

        /// <summary>POST /api/auth/logout — clears the auth cookie.</summary>
        Task<ApiResult> LogoutAsync(CancellationToken cancellationToken = default);

        /// <summary>GET /api/auth/me — the signed-in user, or a 401 failure when the cookie is absent/expired.</summary>
        Task<ApiResult<CurrentUserResponse>> GetCurrentUserAsync(CancellationToken cancellationToken = default);

        /// <summary>GET /api/auth/me/permissions — the signed-in user's effective permission codes.</summary>
        Task<ApiResult<MyPermissionsResponse>> GetMyPermissionsAsync(CancellationToken cancellationToken = default);
    }
}
