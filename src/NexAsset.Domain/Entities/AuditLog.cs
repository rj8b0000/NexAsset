using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class AuditLog : BaseEntity
{
    public Guid? UserId { get; set; }
    public string EntityName { get; set; } = default!;
    public Guid? EntityId { get; set; }
    public string Action { get; set; } = default!;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}
