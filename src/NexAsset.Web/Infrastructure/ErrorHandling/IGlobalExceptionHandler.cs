using System;

namespace NexAsset.Web.Infrastructure.ErrorHandling
{
    /// <summary>
    /// Centralized entry point for handling an unexpected exception: translates it into a
    /// friendly message and surfaces it via <see cref="Notifications.INotificationService"/>.
    /// Intended for use by future API client catch blocks so error handling isn't
    /// re-implemented per page.
    /// </summary>
    public interface IGlobalExceptionHandler
    {
        void Handle(Exception exception, string? context = null);
    }
}
