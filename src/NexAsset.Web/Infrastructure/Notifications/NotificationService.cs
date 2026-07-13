using NexAsset.Web.State;

namespace NexAsset.Web.Infrastructure.Notifications
{
    /// <summary>
    /// Default <see cref="INotificationService"/> implementation. Thin pass-through to the
    /// existing <see cref="NotificationState"/> singleton so every existing Toast/notification
    /// call site continues to work unchanged.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly NotificationState _notificationState;

        public NotificationService(NotificationState notificationState)
        {
            _notificationState = notificationState;
        }

        public void ShowSuccess(string title, string message) => _notificationState.AddToast(title, message, ToastType.Success);

        public void ShowError(string title, string message) => _notificationState.AddToast(title, message, ToastType.Danger);

        public void ShowWarning(string title, string message) => _notificationState.AddToast(title, message, ToastType.Warning);

        public void ShowInfo(string title, string message) => _notificationState.AddToast(title, message, ToastType.Info);

        public void LogActivity(string title, string details, string user = "Admin User") => _notificationState.AddActivity(title, details, user);
    }
}
