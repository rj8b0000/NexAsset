using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class StockMovement : BaseEntity
{
    public Guid InventoryItemId { get; set; }
    public InventoryItem InventoryItem { get; set; } = default!;
    public StockMovementType MovementType { get; set; }
    public int Quantity { get; set; }
    public int StockAfterMovement { get; set; }
    public DateTime MovementAtUtc { get; set; } = DateTime.UtcNow;
    public string? ReferenceNumber { get; set; }
    public string? Remarks { get; set; }
}
