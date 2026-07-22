using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

/// <summary>
/// Represents a draft session for the Project Creation Wizard, persisting incomplete wizard state
/// to enable users to resume project creation without losing data.
/// </summary>
public class DraftSession : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the user who owns this draft session.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the organization ID associated with this draft session.
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the full wizard state serialized as JSON.
    /// Contains all form data from all wizard steps.
    /// </summary>
    public string WizardStateJson { get; set; } = default!;

    /// <summary>
    /// Gets or sets the current step number in the wizard (1-7).
    /// Step 1: General Information
    /// Step 2: Project Team
    /// Step 3: Asset Allocation
    /// Step 4: Project Parameters
    /// Step 5: Documents
    /// Step 6: Review
    /// Step 7: Save
    /// </summary>
    public int CurrentStep { get; set; } = 1;

    /// <summary>
    /// Gets or sets the timestamp of the last successful autosave operation (UTC).
    /// </summary>
    public DateTime LastSavedAtUtc { get; set; } = DateTime.UtcNow;
}
