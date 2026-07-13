using System;

namespace NexAsset.Web.Models.Mock
{
    /// <summary>
    /// In-memory placeholder shape for an asset record.
    /// Backed today by <see cref="NexAsset.Web.Infrastructure.Services.Mock.MockDatabaseService"/> only.
    /// Not wired to NexAsset.API. Expect this shape to be replaced by the real
    /// AssetResponse contract (Guid-keyed, organization/branch/category relations, enum status)
    /// once API integration begins.
    /// </summary>
    public class AssetMock
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public string Serial { get; set; } = "";
        public string Status { get; set; } = "Available"; // Assigned, Available, Maintenance, Disposed
        public decimal Value { get; set; }
        public string Location { get; set; } = "";
        public string? AssignedTo { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
