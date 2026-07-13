using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.DependencyInjection;
using NexAsset.Web.Infrastructure.Authentication;
using NexAsset.Web.Infrastructure.Notifications;

namespace NexAsset.Web.Infrastructure.API
{
    /// <summary>
    /// DelegatingHandler applied to every NexAsset.API-bound HttpClient. Does two things:
    ///
    /// 1. Attaches the ASP.NET Identity auth cookie to each outgoing request via
    ///    <see cref="WebAssemblyHttpRequestMessageExtensions.SetBrowserRequestCredentials"/>
    ///    with <see cref="BrowserRequestCredentials.Include"/>. Blazor WASM issues requests
    ///    through the browser fetch API, which omits cookies for cross-origin calls unless
    ///    credentials are explicitly included — this is what makes cookie auth work at all.
    ///
    /// 2. Centralizes session-expiry handling: a 401 from any *non-auth* endpoint means the
    ///    cookie was rejected mid-session, so it flips the app to logged-out and notifies the
    ///    user once. 401s from the auth endpoints themselves (an anonymous /me probe on startup,
    ///    a bad /login) are expected and left for the caller to interpret.
    ///
    /// The auth state provider and notification service are resolved lazily from
    /// <see cref="IServiceProvider"/> rather than injected, to avoid a construction cycle
    /// (handler → provider → api client → HttpClient → handler).
    /// </summary>
    public sealed class CookieAuthenticationHandler : DelegatingHandler
    {
        private readonly IServiceProvider _services;

        public CookieAuthenticationHandler(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized && !IsAuthEndpoint(request))
            {
                var provider = _services.GetService<NexAssetAuthenticationStateProvider>();
                if (provider is not null && provider.IsCurrentlyAuthenticated)
                {
                    _services.GetService<INotificationService>()?
                        .ShowWarning("Session expired", "Please sign in again to continue.");
                    provider.MarkLoggedOut();
                }
            }

            return response;
        }

        private static bool IsAuthEndpoint(HttpRequestMessage request)
            => request.RequestUri?.AbsolutePath.Contains("/api/auth/", StringComparison.OrdinalIgnoreCase) == true;
    }
}
