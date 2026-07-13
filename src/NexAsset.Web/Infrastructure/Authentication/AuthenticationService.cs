using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Infrastructure.Loading;
using NexAsset.Web.Infrastructure.Notifications;
using NexAsset.Web.Models.Auth;

namespace NexAsset.Web.Infrastructure.Authentication
{
    /// <inheritdoc cref="IAuthenticationService" />
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationApiClient _apiClient;
        private readonly NexAssetAuthenticationStateProvider _stateProvider;
        private readonly INotificationService _notifications;
        private readonly IGlobalLoadingService _loading;

        public AuthenticationService(
            IAuthenticationApiClient apiClient,
            NexAssetAuthenticationStateProvider stateProvider,
            INotificationService notifications,
            IGlobalLoadingService loading)
        {
            _apiClient = apiClient;
            _stateProvider = stateProvider;
            _notifications = notifications;
            _loading = loading;
        }

        public async Task<ApiResult<AuthenticatedUser>> LoginAsync(string email, string password, bool rememberMe, CancellationToken cancellationToken = default)
        {
            using var _ = _loading.BeginOperation();

            var result = await _apiClient.LoginAsync(new LoginRequest(email, password, rememberMe), cancellationToken);
            if (!result.IsSuccess || result.Data is null)
            {
                return ApiResult<AuthenticatedUser>.Failure(result.Error ?? ApiError.FromStatusCode(400));
            }

            var user = AuthenticatedUser.FromLogin(result.Data);
            _stateProvider.MarkAuthenticated(user);
            _notifications.ShowSuccess("Signed in", $"Welcome back, {user.DisplayName}.");
            return ApiResult<AuthenticatedUser>.Success(user);
        }

        public async Task LogoutAsync(CancellationToken cancellationToken = default)
        {
            using var _ = _loading.BeginOperation();

            // Best effort: even if the server call fails (network/expired), still drop local
            // state so the UI can't stay in a stale "signed in" view.
            await _apiClient.LogoutAsync(cancellationToken);
            _stateProvider.MarkLoggedOut();
            _notifications.ShowInfo("Signed out", "You have been signed out.");
        }
    }
}
