using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class PurchaseRequest : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public string RequestNumber { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public Guid RequestedByEmployeeId { get; set; }
    public Employee RequestedByEmployee { get; set; } = default!;
    public DateOnly RequestDate { get; set; }
    public ProcurementStatus Status { get; set; } = ProcurementStatus.PendingApproval;
    public string? ApprovalRemarks { get; set; }
    public Guid? ApprovedByEmployeeId { get; set; }
    public DateTime? ApprovedAtUtc { get; set; }
    public decimal EstimatedAmount { get; set; }
}
