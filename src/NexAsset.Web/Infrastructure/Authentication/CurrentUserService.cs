using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace NexAsset.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Reads the current identity from the app's <see cref="AuthenticationStateProvider"/> and
    /// exposes it as simple synchronous properties. Subscribes to
    /// <see cref="AuthenticationStateProvider.AuthenticationStateChanged"/> so it stays current
    /// through login/logout/session-expiry and re-raises a plain <see cref="OnChange"/> for
    /// non-Razor consumers.
    /// </summary>
    public sealed class CurrentUserService : ICurrentUserService, IDisposable
    {
        private readonly AuthenticationStateProvider _provider;
        private ClaimsPrincipal _user = new(new ClaimsIdentity());

        public CurrentUserService(AuthenticationStateProvider provider)
        {
            _provider = provider;
            _provider.AuthenticationStateChanged += OnAuthenticationStateChanged;
            // Prime the cached principal without blocking construction.
            _ = InitializeAsync();
        }

        public bool IsAuthenticated => _user.Identity?.IsAuthenticated == true;

        public string? UserId => _user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string? Email => _user.FindFirst(ClaimTypes.Email)?.Value;

        public string? DisplayName => _user.Identity?.Name;

        public event Action? OnChange;

        private async Task InitializeAsync()
        {
            var state = await _provider.GetAuthenticationStateAsync();
            _user = state.User;
            OnChange?.Invoke();
        }

        private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            var state = await task;
            _user = state.User;
            OnChange?.Invoke();
        }

        public void Dispose()
        {
            _provider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }
    }
}
