using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Auth;

namespace NexAsset.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Concrete <see cref="IAuthenticationApiClient"/> built on <see cref="ApiClientBase"/>.
    /// Registered as a typed HttpClient whose pipeline attaches the browser's auth cookie
    /// to every request (see <c>ApiServiceCollectionExtensions</c>), so no token plumbing
    /// is needed here — the endpoints just work against the cookie the browser already holds.
    /// </summary>
    public sealed class AuthenticationApiClient : ApiClientBase, IAuthenticationApiClient
    {
        public AuthenticationApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public Task<ApiResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
            => PostAsync<LoginRequest, LoginResponse>("api/auth/login", request, cancellationToken);

        public Task<ApiResult> LogoutAsync(CancellationToken cancellationToken = default)
            => PostAsync("api/auth/logout", cancellationToken);

        public Task<ApiResult<CurrentUserResponse>> GetCurrentUserAsync(CancellationToken cancellationToken = default)
            => GetAsync<CurrentUserResponse>("api/auth/me", cancellationToken);

        public Task<ApiResult<MyPermissionsResponse>> GetMyPermissionsAsync(CancellationToken cancellationToken = default)
            => GetAsync<MyPermissionsResponse>("api/auth/me/permissions", cancellationToken);
    }
}
