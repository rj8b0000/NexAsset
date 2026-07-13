namespace NexAsset.Web.Models.Mock
{
    /// <summary>
    /// In-memory placeholder shape for a maintenance ticket record.
    /// Backed today by <see cref="NexAsset.Web.Infrastructure.Services.Mock.MockDatabaseService"/> only.
    /// Not wired to NexAsset.API (no Maintenance feature exists on the backend yet).
    /// </summary>
    public class MaintenanceMock
    {
        public string Id { get; set; } = "";
        public string AssetName { get; set; } = "";
        public string Issue { get; set; } = "";
        public string Type { get; set; } = "Corrective"; // Preventive, Corrective
        public string Urgency { get; set; } = "Medium"; // Low, Medium, High
        public string Status { get; set; } = "Open"; // Open, In Progress, Resolved
    }
}
