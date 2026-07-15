using System;
using System.Collections.Generic;

namespace NexAsset.Web.Models.Customers
{
    // Wire contracts for /api/enterprise-operations/{customers|service-tickets}.
    // Customers: full CRUD. Service tickets: create + PUT that updates assignment/priority/
    // status/resolution/comments only (title/description are fixed at creation).

    /// <summary>Mirrors NexAsset.Domain.Enums.TicketPriority.</summary>
    public static class TicketPriority
    {
        public const int Low = 1;
        public const int Medium = 2;
        public const int High = 3;
        public const int Critical = 4;

        public static readonly IReadOnlyList<(int Value, string Label)> Options = new List<(int, string)>
        {
            (Low, "Low"), (Medium, "Medium"), (High, "High"), (Critical, "Critical"),
        };

        public static string Label(int value) => value switch
        {
            Low => "Low", Medium => "Medium", High => "High", Critical => "Critical",
            _ => "Unknown"
        };

        public static string BadgeClass(int value) => value switch
        {
            Low => "badge-custom-secondary",
            Medium => "badge-custom-info",
            High => "badge-custom-warning",
            Critical => "badge-custom-danger",
            _ => "badge-custom-secondary"
        };
    }

    /// <summary>Mirrors NexAsset.Domain.Enums.TicketStatus.</summary>
    public static class TicketStatus
    {
        public const int Open = 1;
        public const int Assigned = 2;
        public const int InProgress = 3;
        public const int Resolved = 4;
        public const int Closed = 5;
        public const int Cancelled = 6;

        public static readonly IReadOnlyList<(int Value, string Label)> Options = new List<(int, string)>
        {
            (Open, "Open"), (Assigned, "Assigned"), (InProgress, "In Progress"),
            (Resolved, "Resolved"), (Closed, "Closed"), (Cancelled, "Cancelled"),
        };

        public static string Label(int value) => value switch
        {
            Open => "Open", Assigned => "Assigned", InProgress => "In Progress",
            Resolved => "Resolved", Closed => "Closed", Cancelled => "Cancelled",
            _ => "Unknown"
        };

        public static string BadgeClass(int value) => value switch
        {
            Open => "badge-custom-warning",
            Assigned => "badge-custom-info",
            InProgress => "badge-custom-info",
            Resolved => "badge-custom-success",
            Closed => "badge-custom-secondary",
            Cancelled => "badge-custom-secondary",
            _ => "badge-custom-secondary"
        };
    }

    public sealed record CustomerItem(
        Guid Id, Guid OrganizationId, string Code, string Name, string? ContactPerson,
        string Email, string? Phone, string? Address, bool IsActive);

    /// <summary>Create/edit form model. Required (server-validated): OrganizationId, Code, Name, Email.</summary>
    public sealed class CustomerFormModel
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string? ContactPerson { get; set; }
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; } = true;

        public static CustomerFormModel FromItem(CustomerItem c) => new()
        {
            Id = c.Id, OrganizationId = c.OrganizationId, Code = c.Code, Name = c.Name,
            ContactPerson = c.ContactPerson, Email = c.Email, Phone = c.Phone,
            Address = c.Address, IsActive = c.IsActive
        };
    }

    public sealed record ServiceTicketItem(
        Guid Id, Guid OrganizationId, Guid CustomerId, string TicketNumber, string Title,
        Guid? AssignedToEmployeeId, int Priority, int Status, string? Description,
        string? Resolution, string? Comments);

    /// <summary>Create form model. Required (server-validated): OrganizationId, CustomerId, TicketNumber, Title.</summary>
    public sealed class ServiceTicketCreateModel
    {
        public Guid OrganizationId { get; set; }
        public Guid CustomerId { get; set; }
        public string TicketNumber { get; set; } = "";
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public int Priority { get; set; } = TicketPriority.Medium;
    }

    /// <summary>Body for PUT /service-tickets/{id} (assignment/priority/status/resolution/comments only).</summary>
    public sealed class ServiceTicketUpdateModel
    {
        public Guid? AssignedToEmployeeId { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public string? Resolution { get; set; }
        public string? Comments { get; set; }

        public static ServiceTicketUpdateModel FromItem(ServiceTicketItem t) => new()
        {
            AssignedToEmployeeId = t.AssignedToEmployeeId, Priority = t.Priority,
            Status = t.Status, Resolution = t.Resolution, Comments = t.Comments
        };
    }
}
