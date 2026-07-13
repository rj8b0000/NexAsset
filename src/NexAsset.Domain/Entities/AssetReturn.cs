using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class AssetReturn : BaseEntity
{
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; } = default!;

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    public DateOnly ReturnDate { get; set; }
    public string? InspectionNotes { get; set; }
    public string? ReturnRemarks { get; set; }
    public bool IsAssetUsable { get; set; } = true;
}
