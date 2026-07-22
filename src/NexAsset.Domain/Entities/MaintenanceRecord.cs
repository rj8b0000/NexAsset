using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class MaintenanceRecord : BaseEntity
{
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; } = default!;
    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }
    public MaintenanceType MaintenanceType { get; set; }
    public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Requested;
    public DateOnly RequestedDate { get; set; }
    public DateOnly? ScheduledDate { get; set; }
    public DateOnly? CompletedDate { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public decimal? Cost { get; set; }
    public string? Remarks { get; set; }
}
