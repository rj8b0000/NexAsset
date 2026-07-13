using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Infrastructure.Authentication;
using NexAsset.Web.Infrastructure.Foundation;
using NexAsset.Web.Infrastructure.HR;

namespace NexAsset.Web.Extensions
{
    /// <summary>
    /// DI registration for the API communication layer. This is the one place that configures
    /// how the frontend talks to NexAsset.API, so every current and future typed client shares
    /// the exact same transport: base address, JSON defaults, cookie credentials, and centralized
    /// 401/session-expiry handling.
    ///
    /// Adding a business client later (Organization/Employee/Asset/Vendor) is a single line:
    /// <c>services.AddNexAssetApiClient&lt;IFooApiClient, FooApiClient&gt;(settings);</c> — it
    /// inherits the whole pipeline automatically.
    /// </summary>
    public static class ApiServiceCollectionExtensions
    {
        public static IServiceCollection AddNexAssetApiInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("Api").Get<ApiSettings>() ?? new ApiSettings();
            services.AddSingleton(settings);

            // Cookie-attaching, 401-aware handler shared by every NexAsset.API client.
            services.AddTransient<CookieAuthenticationHandler>();

            // Typed HttpClient for the authentication endpoints. The same helper is reused
            // for every business client so the transport never diverges between features.
            services.AddNexAssetApiClient<IAuthenticationApiClient, AuthenticationApiClient>(settings);

            // Foundation module clients (real HTTP, replacing the former mocks).
            services.AddNexAssetApiClient<IOrganizationApiClient, OrganizationApiClient>(settings);
            services.AddNexAssetApiClient<IBranchApiClient, BranchApiClient>(settings);
            services.AddNexAssetApiClient<IDepartmentApiClient, DepartmentApiClient>(settings);
            services.AddNexAssetApiClient<IDesignationApiClient, DesignationApiClient>(settings);

            // HR module clients (real HTTP). The Employee interface lives in Infrastructure.HR to
            // coexist with the legacy mock IEmployeeApiClient the Asset pages still use.
            services.AddNexAssetApiClient<IEmployeeApiClient, EmployeeApiClient>(settings);
            services.AddNexAssetApiClient<IRoleApiClient, RoleApiClient>(settings);
            services.AddNexAssetApiClient<IPermissionApiClient, PermissionApiClient>(settings);

            return services;
        }

        /// <summary>
        /// Registers a typed API client wired to NexAsset.API with the shared base address,
        /// JSON accept header, and the cookie/401 handler in its pipeline.
        /// </summary>
        public static IServiceCollection AddNexAssetApiClient<TInterface, TImplementation>(
            this IServiceCollection services, ApiSettings settings)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddHttpClient<TInterface, TImplementation>(client =>
                {
                    client.BaseAddress = new Uri(settings.BaseUrl);
                    client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                })
                .AddHttpMessageHandler<CookieAuthenticationHandler>();

            return services;
        }
    }
}
