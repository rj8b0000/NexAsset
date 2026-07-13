using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class Consumable : BaseEntity
{
    public Guid InventoryItemId { get; set; }
    public InventoryItem InventoryItem { get; set; } = default!;
    public string ConsumableCode { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
