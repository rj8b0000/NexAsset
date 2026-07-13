namespace NexAsset.Web.Models.Mock
{
    /// <summary>
    /// In-memory placeholder shape for a procurement/purchase request record.
    /// Backed today by <see cref="NexAsset.Web.Infrastructure.Services.Mock.MockDatabaseService"/> only.
    /// Not wired to NexAsset.API (no Procurement feature exists on the backend yet).
    /// </summary>
    public class ProcurementMock
    {
        public string Id { get; set; } = "";
        public string ItemName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalValue { get; set; }
        public string Requester { get; set; } = "";
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    }
}
