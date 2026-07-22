using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

// Immutable — never updated after insert.
public class ProjectTimelineEvent : BaseEntity
{
    public TimelineEventType EventType { get; set; }
    public string EntityType { get; set; } = default!;   // MaxLength(100)
    public Guid? EntityId { get; set; }
    public string Description { get; set; } = default!;  // MaxLength(500)
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public Guid? UserId { get; set; }
    public string IconType { get; set; } = default!;     // MaxLength(50) — CSS icon class

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
}
