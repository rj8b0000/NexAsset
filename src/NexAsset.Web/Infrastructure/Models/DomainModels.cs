using System;

namespace NexAsset.Web.Infrastructure.Models
{
    public enum ToastType { Success, Warning, Danger, Info }

    public class ToastItem
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public ToastType Type { get; set; } = ToastType.Success;
    }

    public class NotificationItem
    {
        public string Id { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public bool IsRead { get; set; }
        public string Urgency { get; set; } = "Normal"; // High, Medium, Normal
    }

    public class ActivityItem
    {
        public string Id { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public string Title { get; set; } = "";
        public string Details { get; set; } = "";
        public string User { get; set; } = "";
    }

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

    public class EmployeeMock
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Department { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public int AssetsAssigned { get; set; }
        public string Status { get; set; } = "Active"; // Active, On Leave, Terminated
    }

    public class OrganizationMock
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public int HeadCount { get; set; }
        public string Location { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

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

    public class MaintenanceMock
    {
        public string Id { get; set; } = "";
        public string AssetName { get; set; } = "";
        public string Issue { get; set; } = "";
        public string Type { get; set; } = "Corrective"; // Preventive, Corrective
        public string Urgency { get; set; } = "Medium"; // Low, Medium, High
        public string Status { get; set; } = "Open"; // Open, In Progress, Resolved
    }

    public class InvoiceMock
    {
        public string Id { get; set; } = "";
        public string Vendor { get; set; } = "";
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Unpaid"; // Paid, Unpaid, Overdue
    }

    public class AuditLogMock
    {
        public string Id { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public string EntityId { get; set; } = "";
        public string EntityType { get; set; } = "";
        public string Action { get; set; } = "";
        public string Details { get; set; } = "";
        public string User { get; set; } = "";
    }
}
