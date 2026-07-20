using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class ProjectMember : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;
    public string RoleInProject { get; set; } = default!;
    public decimal AllocationPercentage { get; set; }
    public DateOnly JoinedOn { get; set; }
    public DateOnly? ReleasedOn { get; set; }
    public ProjectMemberStatus Status { get; set; } = ProjectMemberStatus.Active;
    public string? Remarks { get; set; }
}
