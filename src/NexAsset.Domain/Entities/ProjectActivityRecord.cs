using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

// Immutable — never updated after insert.
public class ProjectActivityRecord : BaseEntity
{
    public ActivityType ActivityType { get; set; }
    public Guid? UserId { get; set; }
    public string Action { get; set; } = default!;       // MaxLength(500) — human-readable past-tense
    public string TargetEntity { get; set; } = default!; // MaxLength(100) — entity type name
    public Guid? TargetEntityId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Remarks { get; set; }                 // MaxLength(500)

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
}
