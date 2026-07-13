using System;

namespace NexAsset.Web.State
{
    public class ActivityItem
    {
        public string Id { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public string Title { get; set; } = "";
        public string Details { get; set; } = "";
        public string User { get; set; } = "";
    }
}
