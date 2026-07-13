using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class AssetAssignment : BaseEntity
{
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; } = default!;

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    public Guid OrganizationId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? DepartmentId { get; set; }
    public DateOnly AssignedDate { get; set; }
    public DateOnly? UnassignedDate { get; set; }
    public AssetAssignmentStatus Status { get; set; } = AssetAssignmentStatus.Active;
    public string? Remarks { get; set; }
}
