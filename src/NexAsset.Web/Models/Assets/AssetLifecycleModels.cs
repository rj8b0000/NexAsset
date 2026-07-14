using System;
using System.Collections.Generic;

namespace NexAsset.Web.Models.Assets
{
    // Wire contracts for the asset lifecycle endpoints. These are workflow actions + per-asset
    // history (no paged lists on the backend):
    //   POST /api/asset-assignments/assign | /unassign,  GET /api/asset-assignments/assets/{id}/history
    //   POST /api/asset-transfers,                       GET /api/asset-transfers/assets/{id}/history
    //   POST /api/asset-returns,                         GET /api/asset-returns/assets/{id}/history
    // Dates are DateOnly on the backend → "yyyy-MM-dd" strings; enums are ints.

    /// <summary>Mirrors NexAsset.Domain.Enums.AssetAssignmentStatus.</summary>
    public static class AssetAssignmentStatus
    {
        public const int Active = 1;
        public const int Unassigned = 2;
        public const int Returned = 3;
        public const int Transferred = 4;

        public static string Label(int value) => value switch
        {
            Active => "Active",
            Unassigned => "Unassigned",
            Returned => "Returned",
            Transferred => "Transferred",
            _ => "Unknown"
        };
    }

    // --- Commands (request bodies) ---

    public sealed class AssignAssetRequest
    {
        public Guid AssetId { get; set; }
        public Guid EmployeeId { get; set; }
        public string AssignedDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        public string? Remarks { get; set; }
    }

    public sealed class UnassignAssetRequest
    {
        public Guid AssetId { get; set; }
        public string UnassignedDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        public string? Remarks { get; set; }
    }

    public sealed class TransferAssetRequest
    {
        public Guid AssetId { get; set; }
        public Guid? ToEmployeeId { get; set; }
        public Guid? ToBranchId { get; set; }
        public Guid? ToDepartmentId { get; set; }
        public string TransferDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        public bool IsApproved { get; set; } = true;
        public string? Remarks { get; set; }
    }

    public sealed class ReturnAssetRequest
    {
        public Guid AssetId { get; set; }
        public string ReturnDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        public string? InspectionNotes { get; set; }
        public string? ReturnRemarks { get; set; }
        public bool IsAssetUsable { get; set; } = true;
    }

    // --- History records (responses) ---

    public sealed record AssetAssignmentRecord(
        Guid Id, Guid AssetId, Guid EmployeeId, Guid OrganizationId, Guid? BranchId, Guid? DepartmentId,
        DateOnly AssignedDate, DateOnly? UnassignedDate, int Status, string? Remarks);

    public sealed record AssetTransferRecord(
        Guid Id, Guid AssetId, Guid? FromEmployeeId, Guid? ToEmployeeId, Guid? FromBranchId, Guid? ToBranchId,
        Guid? FromDepartmentId, Guid? ToDepartmentId, DateOnly TransferDate, bool IsApproved, string? Remarks);

    public sealed record AssetReturnRecord(
        Guid Id, Guid AssetId, Guid EmployeeId, DateOnly ReturnDate,
        string? InspectionNotes, string? ReturnRemarks, bool IsAssetUsable);

    /// <summary>The three per-asset histories, fetched together for the details dialog.</summary>
    public sealed class AssetHistory
    {
        public List<AssetAssignmentRecord> Assignments { get; init; } = new();
        public List<AssetTransferRecord> Transfers { get; init; } = new();
        public List<AssetReturnRecord> Returns { get; init; } = new();
    }
}
