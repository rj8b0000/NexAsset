using System;
using System.Collections.Generic;

namespace NexAsset.Web.Models.Procurement
{
    // Wire contracts for /api/enterprise-operations/{vendors|purchase-requests|purchase-orders}.
    // Purchase requests/orders are workflow entities: create + status transitions only (no PUT/DELETE
    // on the backend). Enums are ints; DateOnly is "yyyy-MM-dd".

    /// <summary>Mirrors NexAsset.Domain.Enums.ProcurementStatus.</summary>
    public static class ProcurementStatus
    {
        public const int Draft = 1;
        public const int PendingApproval = 2;
        public const int Approved = 3;
        public const int Rejected = 4;
        public const int Cancelled = 5;
        public const int Ordered = 6;

        public static readonly IReadOnlyList<(int Value, string Label)> Options = new List<(int, string)>
        {
            (Draft, "Draft"),
            (PendingApproval, "Pending Approval"),
            (Approved, "Approved"),
            (Rejected, "Rejected"),
            (Cancelled, "Cancelled"),
            (Ordered, "Ordered"),
        };

        public static string Label(int value) => value switch
        {
            Draft => "Draft",
            PendingApproval => "Pending Approval",
            Approved => "Approved",
            Rejected => "Rejected",
            Cancelled => "Cancelled",
            Ordered => "Ordered",
            _ => "Unknown"
        };

        public static string BadgeClass(int value) => value switch
        {
            Draft => "badge-custom-secondary",
            PendingApproval => "badge-custom-warning",
            Approved => "badge-custom-success",
            Rejected => "badge-custom-danger",
            Cancelled => "badge-custom-secondary",
            Ordered => "badge-custom-info",
            _ => "badge-custom-secondary"
        };
    }

    // --- Vendors (full CRUD) ---

    public sealed record VendorItem(
        Guid Id, Guid OrganizationId, string Code, string Name, string? ContactPerson,
        string Email, string? Phone, string? Address, string? TaxNumber, bool IsActive);

    /// <summary>Create/edit form model. Required (server-validated): OrganizationId, Code, Name, Email.
    /// On update the backend binds the same shape and overrides Id from the route.</summary>
    public sealed class VendorFormModel
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string? ContactPerson { get; set; }
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? TaxNumber { get; set; }
        public bool IsActive { get; set; } = true;

        public static VendorFormModel FromItem(VendorItem v) => new()
        {
            Id = v.Id, OrganizationId = v.OrganizationId, Code = v.Code, Name = v.Name,
            ContactPerson = v.ContactPerson, Email = v.Email, Phone = v.Phone,
            Address = v.Address, TaxNumber = v.TaxNumber, IsActive = v.IsActive
        };
    }

    // --- Purchase Requests (create + status workflow) ---

    public sealed record PurchaseRequestItem(
        Guid Id, Guid OrganizationId, string RequestNumber, string Title, Guid RequestedByEmployeeId,
        DateOnly RequestDate, int Status, decimal EstimatedAmount, string? Description, string? ApprovalRemarks);

    public sealed class PurchaseRequestFormModel
    {
        public Guid OrganizationId { get; set; }
        public string RequestNumber { get; set; } = "";
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public Guid RequestedByEmployeeId { get; set; }
        public string RequestDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        public decimal EstimatedAmount { get; set; }
    }

    // --- Purchase Orders (create + status workflow) ---

    public sealed record PurchaseOrderItem(
        Guid Id, Guid OrganizationId, string OrderNumber, Guid? PurchaseRequestId, Guid VendorId,
        DateOnly OrderDate, DateOnly? ExpectedDeliveryDate, int Status, decimal TotalAmount, string? Remarks);

    public sealed class PurchaseOrderFormModel
    {
        public Guid OrganizationId { get; set; }
        public string OrderNumber { get; set; } = "";
        public Guid? PurchaseRequestId { get; set; }
        public Guid VendorId { get; set; }
        public string OrderDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        public string? ExpectedDeliveryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Remarks { get; set; }
    }

    /// <summary>
    /// Body for the status endpoints. PR uses <c>ApprovalRemarks</c>, PO uses <c>Remarks</c> —
    /// both properties are carried; the backend binds the one its command declares.
    /// </summary>
    public sealed class SetProcurementStatusRequest
    {
        public int Status { get; set; }
        public Guid? ApprovedByEmployeeId { get; set; }
        public string? ApprovalRemarks { get; set; }
        public string? Remarks { get; set; }
    }
}
