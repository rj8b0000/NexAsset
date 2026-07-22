using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class ProjectAssetAllocation : BaseEntity
{
    public DateOnly AllocationDate { get; set; }
    public DateOnly? ReturnDate { get; set; }
    public int AllocatedQuantity { get; set; }
    public int ReturnedQuantity { get; set; }
    public AllocationStatus Status { get; set; } = AllocationStatus.Active;
    public string? Remarks { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;

    public Guid AssetId { get; set; }
    public Asset Asset { get; set; } = default!;
}
