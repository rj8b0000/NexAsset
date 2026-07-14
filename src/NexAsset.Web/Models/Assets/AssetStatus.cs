using System.Collections.Generic;

namespace NexAsset.Web.Models.Assets
{
    /// <summary>
    /// Mirrors NexAsset.Domain.Enums.AssetStatus. Serialized as its integer value by the API
    /// (no string-enum converter), so the frontend sends/reads ints.
    /// </summary>
    public static class AssetStatus
    {
        public const int Available = 1;
        public const int Assigned = 2;
        public const int InTransfer = 3;
        public const int InMaintenance = 4;
        public const int Retired = 5;
        public const int Lost = 6;
        public const int Damaged = 7;

        public static readonly IReadOnlyList<(int Value, string Label)> Options = new List<(int, string)>
        {
            (Available, "Available"),
            (Assigned, "Assigned"),
            (InTransfer, "In Transfer"),
            (InMaintenance, "In Maintenance"),
            (Retired, "Retired"),
            (Lost, "Lost"),
            (Damaged, "Damaged"),
        };

        public static string Label(int value) => value switch
        {
            Available => "Available",
            Assigned => "Assigned",
            InTransfer => "In Transfer",
            InMaintenance => "In Maintenance",
            Retired => "Retired",
            Lost => "Lost",
            Damaged => "Damaged",
            _ => "Unknown"
        };

        /// <summary>Badge style class for the status, matching the existing design system.</summary>
        public static string BadgeClass(int value) => value switch
        {
            Available => "badge-custom-success",
            Assigned => "badge-custom-info",
            InTransfer => "badge-custom-warning",
            InMaintenance => "badge-custom-warning",
            Retired => "badge-custom-secondary",
            Lost => "badge-custom-danger",
            Damaged => "badge-custom-danger",
            _ => "badge-custom-secondary"
        };
    }
}
