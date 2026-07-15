using System;
using System.Collections.Generic;

namespace NexAsset.Web.Models.Inventory
{
    // Wire contracts for /api/enterprise-operations/{inventory|consumables}. Stock changes go
    // through POST /inventory/stock-movements (the movement adjusts CurrentStock server-side);
    // per-item history via GET /inventory/{id}/stock-history. Enums are ints.
    //
    // NOTE: Create/Update accept a Description, but InventoryItemDto does NOT return it — the
    // edit form therefore cannot prefill Description (documented backend mismatch).

    /// <summary>Mirrors NexAsset.Domain.Enums.StockMovementType.</summary>
    public static class StockMovementType
    {
        public const int StockIn = 1;
        public const int StockOut = 2;
        public const int Adjustment = 3;
        public const int Reserved = 4;
        public const int Released = 5;

        public static readonly IReadOnlyList<(int Value, string Label)> Options = new List<(int, string)>
        {
            (StockIn, "Stock In"),
            (StockOut, "Stock Out"),
            (Adjustment, "Adjustment"),
            (Reserved, "Reserved"),
            (Released, "Released"),
        };

        public static string Label(int value) => value switch
        {
            StockIn => "Stock In",
            StockOut => "Stock Out",
            Adjustment => "Adjustment",
            Reserved => "Reserved",
            Released => "Released",
            _ => "Unknown"
        };

        public static string Icon(int value) => value switch
        {
            StockIn => "lucide-arrow-down-to-line",
            StockOut => "lucide-arrow-up-from-line",
            Adjustment => "lucide-sliders",
            Reserved => "lucide-lock",
            Released => "lucide-unlock",
            _ => "lucide-package"
        };
    }

    public sealed record InventoryItemModel(
        Guid Id, Guid OrganizationId, Guid? BranchId, string ItemCode, string ItemName,
        int CurrentStock, int ReservedStock, int AvailableStock, int ReorderLevel,
        string UnitOfMeasure, bool IsActive)
    {
        public bool IsLowStock => CurrentStock <= ReorderLevel;
    }

    /// <summary>Create/edit form model. Required (server-validated): OrganizationId, ItemCode, ItemName, UnitOfMeasure.</summary>
    public sealed class InventoryItemFormModel
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid? BranchId { get; set; }
        public string ItemCode { get; set; } = "";
        public string ItemName { get; set; } = "";
        public string? Description { get; set; }
        public int CurrentStock { get; set; }
        public int ReservedStock { get; set; }
        public int ReorderLevel { get; set; }
        public string UnitOfMeasure { get; set; } = "pcs";
        public bool IsActive { get; set; } = true;

        public static InventoryItemFormModel FromItem(InventoryItemModel i) => new()
        {
            Id = i.Id, OrganizationId = i.OrganizationId, BranchId = i.BranchId, ItemCode = i.ItemCode,
            ItemName = i.ItemName, CurrentStock = i.CurrentStock, ReservedStock = i.ReservedStock,
            ReorderLevel = i.ReorderLevel, UnitOfMeasure = i.UnitOfMeasure, IsActive = i.IsActive
        };
    }

    /// <summary>Body for POST /inventory/stock-movements.</summary>
    public sealed class StockMovementFormModel
    {
        public Guid InventoryItemId { get; set; }
        public int MovementType { get; set; } = StockMovementType.StockIn;
        public int Quantity { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Remarks { get; set; }
    }

    public sealed record StockMovementRecord(
        Guid Id, Guid InventoryItemId, int MovementType, int Quantity, int StockAfterMovement,
        DateTime MovementAtUtc, string? ReferenceNumber, string? Remarks);

    public sealed record ConsumableItem(
        Guid Id, Guid InventoryItemId, string ConsumableCode, string Name, string? Description, bool IsActive);

    /// <summary>Create/edit form model. Required (server-validated): InventoryItemId, ConsumableCode, Name.</summary>
    public sealed class ConsumableFormModel
    {
        public Guid Id { get; set; }
        public Guid InventoryItemId { get; set; }
        public string ConsumableCode { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public static ConsumableFormModel FromItem(ConsumableItem c) => new()
        {
            Id = c.Id, InventoryItemId = c.InventoryItemId, ConsumableCode = c.ConsumableCode,
            Name = c.Name, Description = c.Description, IsActive = c.IsActive
        };
    }
}
