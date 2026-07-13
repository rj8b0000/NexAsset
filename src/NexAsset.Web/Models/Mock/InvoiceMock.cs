using System;

namespace NexAsset.Web.Models.Mock
{
    /// <summary>
    /// In-memory placeholder shape for a finance/invoice record.
    /// Backed today by <see cref="NexAsset.Web.Infrastructure.Services.Mock.MockDatabaseService"/> only.
    /// Not wired to NexAsset.API (no Finance feature exists on the backend yet).
    /// </summary>
    public class InvoiceMock
    {
        public string Id { get; set; } = "";
        public string Vendor { get; set; } = "";
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Unpaid"; // Paid, Unpaid, Overdue
    }
}
