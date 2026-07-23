using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class DraftSession : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid OrganizationId { get; set; }
    public string WizardStateJson { get; set; } = default!;
    public int CurrentStep { get; set; } = 1;
    public DateTime LastSavedAtUtc { get; set; } = DateTime.UtcNow;
}
