using System;
using NexAsset.Web.Infrastructure.Notifications;

namespace NexAsset.Web.Infrastructure.ErrorHandling
{
    /// <summary>
    /// Default <see cref="IGlobalExceptionHandler"/>: translates the exception to a friendly
    /// message via <see cref="ApiErrorTranslator"/> and shows it as an error toast through the
    /// existing notification pipeline. Not called anywhere yet — nothing in this phase performs
    /// real HTTP calls that would need it.
    /// </summary>
    public class GlobalExceptionHandler : IGlobalExceptionHandler
    {
        private readonly INotificationService _notifications;

        public GlobalExceptionHandler(INotificationService notifications)
        {
            _notifications = notifications;
        }

        public void Handle(Exception exception, string? context = null)
        {
            var message = ApiErrorTranslator.ToFriendlyMessage(exception);
            var title = string.IsNullOrEmpty(context) ? "Something went wrong" : context;
            _notifications.ShowError(title, message);
        }
    }
}
