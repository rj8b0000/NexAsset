using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class ProjectAssetAllocation : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; } = default!;
    public int AllocatedQuantity { get; set; }
    public int ReturnedQuantity { get; set; }
    public DateOnly AllocatedOn { get; set; }
    public DateOnly? ReturnedOn { get; set; }
    public ProjectAssetAllocationStatus Status { get; set; } = ProjectAssetAllocationStatus.Allocated;
    public string? Remarks { get; set; }
}
