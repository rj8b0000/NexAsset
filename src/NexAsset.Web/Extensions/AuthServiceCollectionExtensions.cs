using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NexAsset.Web.Infrastructure.Authentication;

namespace NexAsset.Web.Extensions
{
    /// <summary>
    /// DI registration for the authentication layer. Wires the Blazor authorization stack
    /// (<c>AddAuthorizationCore</c> + a real <see cref="AuthenticationStateProvider"/>) to the
    /// backend-driven <see cref="NexAssetAuthenticationStateProvider"/>, plus the orchestration
    /// (<see cref="IAuthenticationService"/>) and read-model (<see cref="ICurrentUserService"/>)
    /// that UI components consume.
    ///
    /// The provider is registered once and exposed under both its concrete type (needed by the
    /// auth service and the 401 handler) and the framework's <see cref="AuthenticationStateProvider"/>
    /// base type (needed by <c>&lt;AuthorizeView&gt;</c> / <c>CascadingAuthenticationState</c>), so
    /// every consumer shares the same instance and the same cached session.
    /// </summary>
    public static class AuthServiceCollectionExtensions
    {
        public static IServiceCollection AddNexAssetAuthentication(this IServiceCollection services)
        {
            services.AddAuthorizationCore();

            services.AddScoped<NexAssetAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(sp =>
                sp.GetRequiredService<NexAssetAuthenticationStateProvider>());

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IPermissionChecker, PermissionChecker>();

            return services;
        }
    }
}
