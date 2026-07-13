using System;
using System.Collections.Generic;
using System.Linq;

namespace NexAsset.Web.State
{
    public class NotificationState
    {
        public List<ToastItem> Toasts { get; } = new();
        public List<NotificationItem> Notifications { get; } = new();
        public List<ActivityItem> Activities { get; } = new();

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddToast(string title, string message, ToastType type = ToastType.Success)
        {
            var id = Guid.NewGuid().ToString();
            var toast = new ToastItem { Id = id, Title = title, Message = message, Type = type };
            Toasts.Add(toast);
            NotifyStateChanged();
            
            // Auto remove after 5 seconds
            System.Threading.Tasks.Task.Delay(5000).ContinueWith(_ =>
            {
                Toasts.RemoveAll(t => t.Id == id);
                NotifyStateChanged();
            });
        }

        public void RemoveToast(string id)
        {
            Toasts.RemoveAll(t => t.Id == id);
            NotifyStateChanged();
        }

        public void AddActivity(string title, string details, string user = "Admin User")
        {
            Activities.Insert(0, new ActivityItem 
            { 
                Id = Guid.NewGuid().ToString(),
                Timestamp = DateTime.Now,
                Title = title, 
                Details = details,
                User = user
            });
            if (Activities.Count > 50) Activities.RemoveAt(Activities.Count - 1);
            NotifyStateChanged();
        }

        public void AddNotification(string title, string message, string urgency = "Normal")
        {
            Notifications.Insert(0, new NotificationItem
            {
                Id = Guid.NewGuid().ToString(),
                Timestamp = DateTime.Now,
                Title = title,
                Message = message,
                IsRead = false,
                Urgency = urgency
            });
            NotifyStateChanged();
        }

        public void MarkAllNotificationsRead()
        {
            foreach (var n in Notifications) n.IsRead = true;
            NotifyStateChanged();
        }

        public void ClearNotifications()
        {
            Notifications.Clear();
            NotifyStateChanged();
        }
    }
}
