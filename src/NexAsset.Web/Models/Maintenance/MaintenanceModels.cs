using System;
using System.Collections.Generic;

namespace NexAsset.Web.Models.Maintenance
{
    // Wire contracts for /api/enterprise-operations/maintenance. Workflow entity: create +
    // status transitions (no PUT/DELETE on the backend). Enums are ints; DateOnly "yyyy-MM-dd".

    /// <summary>Mirrors NexAsset.Domain.Enums.MaintenanceType.</summary>
    public static class MaintenanceType
    {
        public const int Preventive = 1;
        public const int Corrective = 2;

        public static readonly IReadOnlyList<(int Value, string Label)> Options = new List<(int, string)>
        {
            (Preventive, "Preventive"),
            (Corrective, "Corrective"),
        };

        public static string Label(int value) => value switch
        {
            Preventive => "Preventive",
            Corrective => "Corrective",
            _ => "Unknown"
        };
    }

    /// <summary>Mirrors NexAsset.Domain.Enums.MaintenanceStatus.</summary>
    public static class MaintenanceStatus
    {
        public const int Requested = 1;
        public const int Scheduled = 2;
        public const int InProgress = 3;
        public const int Completed = 4;
        public const int Cancelled = 5;

        public static readonly IReadOnlyList<(int Value, string Label)> Options = new List<(int, string)>
        {
            (Requested, "Requested"),
            (Scheduled, "Scheduled"),
            (InProgress, "In Progress"),
            (Completed, "Completed"),
            (Cancelled, "Cancelled"),
        };

        public static string Label(int value) => value switch
        {
            Requested => "Requested",
            Scheduled => "Scheduled",
            InProgress => "In Progress",
            Completed => "Completed",
            Cancelled => "Cancelled",
            _ => "Unknown"
        };

        public static string BadgeClass(int value) => value switch
        {
            Requested => "badge-custom-warning",
            Scheduled => "badge-custom-info",
            InProgress => "badge-custom-info",
            Completed => "badge-custom-success",
            Cancelled => "badge-custom-secondary",
            _ => "badge-custom-secondary"
        };
    }

    public sealed record MaintenanceRecordItem(
        Guid Id, Guid AssetId, int MaintenanceType, int Status, DateOnly RequestedDate,
        DateOnly? ScheduledDate, DateOnly? CompletedDate, string Title, string? Description,
        decimal? Cost, string? Remarks);

    /// <summary>Create form model. Required (server-validated): AssetId, Title, RequestedDate, MaintenanceType.</summary>
    public sealed class MaintenanceFormModel
    {
        public Guid AssetId { get; set; }
        public int MaintenanceType { get; set; } = Maintenance.MaintenanceType.Corrective;
        public string RequestedDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        public string? ScheduledDate { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public decimal? Cost { get; set; }
        public string? Remarks { get; set; }
    }

    /// <summary>Body for POST /maintenance/{id}/status.</summary>
    public sealed class MaintenanceStatusRequest
    {
        public int Status { get; set; }
        public string? ScheduledDate { get; set; }
        public string? CompletedDate { get; set; }
        public string? Remarks { get; set; }
    }
}
