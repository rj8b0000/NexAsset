using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class ProjectTeamMember : BaseEntity
{
    public string ProjectRole { get; set; } = default!;
    public int AllocationPercentage { get; set; }
    public DateOnly JoinedDate { get; set; }
    public DateOnly? ReleasedDate { get; set; }
    public TeamMemberStatus Status { get; set; } = TeamMemberStatus.Active;
    public string? Remarks { get; set; }

    // Snapshots (captured at time of assignment — not updated if employee changes)
    public Guid? SnapshotBranchId { get; set; }
    public Guid? SnapshotDepartmentId { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;
}
