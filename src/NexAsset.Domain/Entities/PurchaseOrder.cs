using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class PurchaseOrder : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public string OrderNumber { get; set; } = default!;
    public Guid? PurchaseRequestId { get; set; }
    public PurchaseRequest? PurchaseRequest { get; set; }
    public Guid VendorId { get; set; }
    public Vendor Vendor { get; set; } = default!;
    public DateOnly OrderDate { get; set; }
    public DateOnly? ExpectedDeliveryDate { get; set; }
    public ProcurementStatus Status { get; set; } = ProcurementStatus.PendingApproval;
    public decimal TotalAmount { get; set; }
    public string? Remarks { get; set; }
    public Guid? ApprovedByEmployeeId { get; set; }
    public DateTime? ApprovedAtUtc { get; set; }
}
