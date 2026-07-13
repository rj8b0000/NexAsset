using Microsoft.Extensions.DependencyInjection;
using NexAsset.Web.Infrastructure.ErrorHandling;
using NexAsset.Web.Infrastructure.Loading;
using NexAsset.Web.Infrastructure.Notifications;

namespace NexAsset.Web.Extensions
{
    /// <summary>
    /// DI registration for cross-cutting infrastructure: notifications, centralized error
    /// handling, and the global loading tracker. All additive — none of this replaces or
    /// changes the existing NotificationState/Toast pipeline that pages currently use.
    /// </summary>
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddNexAssetCoreInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IGlobalExceptionHandler, GlobalExceptionHandler>();
            services.AddSingleton<IGlobalLoadingService, GlobalLoadingService>();

            return services;
        }
    }
}
