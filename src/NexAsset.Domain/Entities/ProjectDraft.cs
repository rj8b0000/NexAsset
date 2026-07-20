using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class ProjectDraft : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;
    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }
    public Guid OwnerEmployeeId { get; set; }
    public Employee OwnerEmployee { get; set; } = default!;
    public int CurrentStep { get; set; } = 1;
    public string DraftName { get; set; } = default!;
    public string? DraftState { get; set; }
    public DateTime LastSavedAtUtc { get; set; } = DateTime.UtcNow;
    public bool IsSubmitted { get; set; }
}
