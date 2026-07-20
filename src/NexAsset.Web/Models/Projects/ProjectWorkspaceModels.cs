using System;
using System.Collections.Generic;

namespace NexAsset.Web.Models.Projects
{
    public static class ProjectPriority
    {
        public const int Low = 1;
        public const int Medium = 2;
        public const int High = 3;
        public const int Critical = 4;

        public static readonly IReadOnlyList<(int Value, string Label)> Options = new List<(int, string)>
        {
            (Low, "Low"), (Medium, "Medium"), (High, "High"), (Critical, "Critical")
        };

        public static string Label(int value) => value switch
        {
            Low => "Low",
            Medium => "Medium",
            High => "High",
            Critical => "Critical",
            _ => "Unknown"
        };
    }

    public static class ProjectStatus
    {
        public const int Draft = 1;
        public const int Planned = 2;
        public const int Active = 3;
        public const int OnHold = 4;
        public const int Completed = 5;
        public const int Cancelled = 6;
        public const int Archived = 7;

        public static readonly IReadOnlyList<(int Value, string Label)> Options = new List<(int, string)>
        {
            (Draft, "Draft"), (Planned, "Planned"), (Active, "Active"), (OnHold, "On Hold"),
            (Completed, "Completed"), (Cancelled, "Cancelled"), (Archived, "Archived")
        };

        public static string Label(int value) => value switch
        {
            Draft => "Draft",
            Planned => "Planned",
            Active => "Active",
            OnHold => "On Hold",
            Completed => "Completed",
            Cancelled => "Cancelled",
            Archived => "Archived",
            _ => "Unknown"
        };

        public static string BadgeClass(int value) => value switch
        {
            Draft => "badge-custom-secondary",
            Planned => "badge-custom-info",
            Active => "badge-custom-success",
            OnHold => "badge-custom-warning",
            Completed => "badge-custom-primary",
            Cancelled => "badge-custom-danger",
            Archived => "badge-custom-secondary",
            _ => "badge-custom-secondary"
        };
    }

    public static class ProjectParameterInputType
    {
        public static readonly IReadOnlyList<(int Value, string Label)> Options = new List<(int, string)>
        {
            (1, "Text"), (2, "Number"), (3, "Decimal"), (4, "Date"), (5, "Dropdown"),
            (6, "Checkbox"), (7, "Textarea"), (8, "Email"), (9, "Phone"), (10, "URL")
        };
    }

    public sealed record ProjectCategoryItem(Guid Id, Guid OrganizationId, string Name, string? Description, bool IsSystemSuggested, bool IsActive);
    public sealed record ProjectItem(Guid Id, Guid OrganizationId, Guid CategoryId, Guid? ClientId, Guid BranchId, Guid DepartmentId, Guid ProjectManagerId, string ProjectName, string? Description, int Priority, int Status, DateOnly StartDate, DateOnly? EndDate, DateOnly? ExpectedCompletion, string? Notes, bool IsArchived, DateTime? ArchivedAtUtc);
    public sealed record ProjectMemberItem(Guid Id, Guid ProjectId, Guid EmployeeId, string RoleInProject, decimal AllocationPercentage, DateOnly JoinedOn, DateOnly? ReleasedOn, int Status, string? Remarks);
    public sealed record ProjectAssetAllocationItem(Guid Id, Guid ProjectId, Guid AssetId, int AllocatedQuantity, int ReturnedQuantity, DateOnly AllocatedOn, DateOnly? ReturnedOn, int Status, string? Remarks);
    public sealed record ProjectParameterGroupItem(Guid Id, Guid ProjectId, string GroupName, int DisplayOrder);
    public sealed record ProjectParameterItem(Guid Id, Guid ProjectId, Guid GroupId, string ParameterName, int InputType, string? Value, string? Unit, bool Required, int DisplayOrder, bool IsVisible);
    public sealed record ProjectDocumentItem(Guid Id, Guid ProjectId, string Category, string DocumentName, string FilePath, Guid UploadedBy, DateTime UploadedOn, int Version, DateOnly? ExpiryDate, string? Remarks);
    public sealed record ProjectActivityItem(Guid Id, Guid ProjectId, string ActivityType, string Message, Guid? ActorEmployeeId, DateTime OccurredAtUtc);
    public sealed record ProjectDraftItem(Guid Id, Guid OrganizationId, Guid? ProjectId, Guid OwnerEmployeeId, int CurrentStep, string DraftName, string? DraftState, DateTime LastSavedAtUtc, bool IsSubmitted);

    public sealed record ProjectDashboardKpiItem(decimal CompletionPercentage, string HealthStatus, int AssetsAllocated, int EmployeesAssigned, int ConsumablesUsed, int DocumentsUploaded, int OpenRisks, int PendingApprovals, int UpcomingDeadlines, int MaintenanceRequests, decimal BudgetUtilizationPercentage);
    
    public sealed record ProjectBudgetItem(Guid Id, Guid ProjectId, decimal EstimatedBudget, decimal ActualCost, decimal ProcurementCost, decimal MaintenanceCost, decimal LabourCost, decimal MiscellaneousCost, DateTime UpdatedAtUtc);
    public sealed record ProjectRiskItem(Guid Id, Guid ProjectId, string Title, string? Description, string Probability, string Impact, string Severity, string? MitigationPlan, Guid? OwnerEmployeeId, string Status, DateTime? ClosedDate, DateTime CreatedAtUtc);
    public sealed record ProjectSettingItem(Guid Id, Guid ProjectId, string Key, string Value, string? Description);

    public sealed class ProjectFormModel
    {
        public Guid Id { get; set; }
        public string OrganizationId { get; set; } = "";
        public string CategoryId { get; set; } = "";
        public string? ClientId { get; set; }
        public string BranchId { get; set; } = "";
        public string DepartmentId { get; set; } = "";
        public string ProjectManagerId { get; set; } = "";
        public string ProjectName { get; set; } = "";
        public string? Description { get; set; }
        public int Priority { get; set; } = ProjectPriority.Medium;
        public int Status { get; set; } = ProjectStatus.Draft;
        public string StartDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        public string? EndDate { get; set; }
        public string? ExpectedCompletion { get; set; }
        public string? Notes { get; set; }

        public static ProjectFormModel FromItem(ProjectItem item) => new()
        {
            Id = item.Id,
            OrganizationId = item.OrganizationId.ToString(),
            CategoryId = item.CategoryId.ToString(),
            ClientId = item.ClientId?.ToString(),
            BranchId = item.BranchId.ToString(),
            DepartmentId = item.DepartmentId.ToString(),
            ProjectManagerId = item.ProjectManagerId.ToString(),
            ProjectName = item.ProjectName,
            Description = item.Description,
            Priority = item.Priority,
            Status = item.Status,
            StartDate = item.StartDate.ToString("yyyy-MM-dd"),
            EndDate = item.EndDate?.ToString("yyyy-MM-dd"),
            ExpectedCompletion = item.ExpectedCompletion?.ToString("yyyy-MM-dd"),
            Notes = item.Notes
        };
    }

    public sealed class ProjectDraftFormModel
    {
        public Guid? Id { get; set; }
        public string OrganizationId { get; set; } = "";
        public Guid? ProjectId { get; set; }
        public string OwnerEmployeeId { get; set; } = "";
        public int CurrentStep { get; set; } = 1;
        public string DraftName { get; set; } = "";
        public string? DraftState { get; set; }
    }

    public sealed class ProjectMemberFormModel
    {
        public Guid ProjectId { get; set; }
        public Guid EmployeeId { get; set; }
        public string RoleInProject { get; set; } = "";
        public decimal AllocationPercentage { get; set; } = 100;
        public string JoinedOn { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        public string? Remarks { get; set; }
    }

    public sealed class ProjectAssetAllocationFormModel
    {
        public Guid ProjectId { get; set; }
        public Guid AssetId { get; set; }
        public int AllocatedQuantity { get; set; } = 1;
        public string AllocatedOn { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        public string? Remarks { get; set; }
    }

    public sealed class ProjectParameterGroupFormModel
    {
        public Guid ProjectId { get; set; }
        public string GroupName { get; set; } = "Technical";
        public int DisplayOrder { get; set; } = 1;
    }

    public sealed class ProjectParameterFormModel
    {
        public Guid ProjectId { get; set; }
        public Guid GroupId { get; set; }
        public string ParameterName { get; set; } = "";
        public int InputType { get; set; } = 1;
        public string? Value { get; set; }
        public string? Unit { get; set; }
        public bool Required { get; set; }
        public int DisplayOrder { get; set; } = 1;
        public bool IsVisible { get; set; } = true;
    }

    public sealed class ProjectDocumentFormModel
    {
        public Guid ProjectId { get; set; }
        public string Category { get; set; } = "Other";
        public string DocumentName { get; set; } = "";
        public string FilePath { get; set; } = "";
        public Guid UploadedBy { get; set; }
        public string? ExpiryDate { get; set; }
        public string? Remarks { get; set; }
    }

    public sealed class ProjectBudgetFormModel
    {
        public Guid ProjectId { get; set; }
        public decimal EstimatedBudget { get; set; }
        public decimal ActualCost { get; set; }
        public decimal ProcurementCost { get; set; }
        public decimal MaintenanceCost { get; set; }
        public decimal LabourCost { get; set; }
        public decimal MiscellaneousCost { get; set; }
    }

    public sealed class ProjectRiskFormModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string Probability { get; set; } = "Low";
        public string Impact { get; set; } = "Low";
        public string Severity { get; set; } = "Low";
        public string? MitigationPlan { get; set; }
        public Guid? OwnerEmployeeId { get; set; }
        public string Status { get; set; } = "Open";
    }

    public sealed class ProjectSettingFormModel
    {
        public Guid ProjectId { get; set; }
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
        public string? Description { get; set; }
    }
}
