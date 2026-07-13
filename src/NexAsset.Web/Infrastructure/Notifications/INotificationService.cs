namespace NexAsset.Web.Infrastructure.Notifications
{
    /// <summary>
    /// Service-oriented facade over the existing <see cref="NexAsset.Web.State.NotificationState"/>
    /// toast/notification/activity feed, so feature and infrastructure code (including the
    /// future API layer) can depend on an interface instead of the concrete state container.
    /// Forwards to NotificationState — behavior is unchanged.
    /// </summary>
    public interface INotificationService
    {
        void ShowSuccess(string title, string message);
        void ShowError(string title, string message);
        void ShowWarning(string title, string message);
        void ShowInfo(string title, string message);
        void LogActivity(string title, string details, string user = "Admin User");
    }
}
