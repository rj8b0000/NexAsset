using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class AssetTransfer : BaseEntity
{
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; } = default!;

    public Guid? FromEmployeeId { get; set; }
    public Guid? ToEmployeeId { get; set; }
    public Guid? FromBranchId { get; set; }
    public Guid? ToBranchId { get; set; }
    public Guid? FromDepartmentId { get; set; }
    public Guid? ToDepartmentId { get; set; }
    public DateOnly TransferDate { get; set; }
    public bool IsApproved { get; set; }
    public string? Remarks { get; set; }
}
