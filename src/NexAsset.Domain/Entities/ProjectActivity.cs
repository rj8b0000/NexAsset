using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class ProjectActivity : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public string ActivityType { get; set; } = default!;
    public string Message { get; set; } = default!;
    public Guid? ActorEmployeeId { get; set; }
    public Employee? ActorEmployee { get; set; }
    public DateTime OccurredAtUtc { get; set; } = DateTime.UtcNow;
}
