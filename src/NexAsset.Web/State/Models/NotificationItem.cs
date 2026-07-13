using System;

namespace NexAsset.Web.State
{
    public class NotificationItem
    {
        public string Id { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public bool IsRead { get; set; }
        public string Urgency { get; set; } = "Normal"; // High, Medium, Normal
    }
}
