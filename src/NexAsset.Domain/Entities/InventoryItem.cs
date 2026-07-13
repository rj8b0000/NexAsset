using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class InventoryItem : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid? BranchId { get; set; }
    public Branch? Branch { get; set; }
    public string ItemCode { get; set; } = default!;
    public string ItemName { get; set; } = default!;
    public string? Description { get; set; }
    public int CurrentStock { get; set; }
    public int ReservedStock { get; set; }
    public int ReorderLevel { get; set; }
    public string UnitOfMeasure { get; set; } = "Each";
    public bool IsActive { get; set; } = true;
    public int AvailableStock => CurrentStock - ReservedStock;
}
