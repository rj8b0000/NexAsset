using System;

namespace NexAsset.Web.Models.Mock
{
    /// <summary>
    /// In-memory placeholder shape for an audit log entry.
    /// Backed today by <see cref="NexAsset.Web.Infrastructure.Services.Mock.MockDatabaseService"/> only.
    /// Not wired to NexAsset.API.
    /// </summary>
    public class AuditLogMock
    {
        public string Id { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public string EntityId { get; set; } = "";
        public string EntityType { get; set; } = "";
        public string Action { get; set; } = "";
        public string Details { get; set; } = "";
        public string User { get; set; } = "";
    }
}
