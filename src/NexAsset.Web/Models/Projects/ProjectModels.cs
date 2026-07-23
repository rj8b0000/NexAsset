using System;
using System.Collections.Generic;

namespace NexAsset.Web.Models.Projects;

public class ProjectListItem
{
    public Guid Id { get; set; }
    public string ProjectCode { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; }
    public ProjectPriority Priority { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? ProjectManagerName { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public DateOnly? ExpectedCompletionDate { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}

public class ProjectDetailModel
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string ProjectCode { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? InternalRemarks { get; set; }
    public ProjectStatus Status { get; set; }
    public ProjectPriority Priority { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public DateOnly? ExpectedCompletionDate { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public Guid? BranchId { get; set; }
    public string? BranchName { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public Guid? ProjectManagerEmployeeId { get; set; }
    public string? ProjectManagerName { get; set; }
    public string? Location { get; set; }
    public decimal? Budget { get; set; }
    public string? Currency { get; set; } = "USD";
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}

public class ProjectTeamMemberModel
{
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeCode { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string DesignationName { get; set; } = string.Empty;
    public string ProjectRole { get; set; } = "Member";
    public int AllocationPercentage { get; set; } = 100;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Status { get; set; } = "Active";
    public string? Remarks { get; set; }
}

public class ProjectAssetAllocationModel
{
    public Guid AssetId { get; set; }
    public string AssetCode { get; set; } = string.Empty;
    public string AssetName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public int AvailableQuantity { get; set; } = 1;
    public int AllocatedQuantity { get; set; } = 1;
    public string Unit { get; set; } = "Units";
    public string Status { get; set; } = "Allocated";
    public string? AllocatedBy { get; set; }
    public DateTime AllocatedDateUtc { get; set; } = DateTime.UtcNow;
    public int ReturnedQuantity { get; set; } = 0;
}

public class ProjectFormModel
{
    public Guid OrganizationId { get; set; }
    public string ProjectCode { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DetailedDescription { get; set; }
    public string? Notes { get; set; }
    public string? InternalRemarks { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? ProjectManagerEmployeeId { get; set; }
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    public ProjectStatus Status { get; set; } = ProjectStatus.Planning;
    public string? Location { get; set; }
    public decimal? Budget { get; set; }
    public string Currency { get; set; } = "USD";
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public DateOnly? ExpectedCompletionDate { get; set; }
    public string? ProjectThumbnailUrl { get; set; }

    public List<ProjectTeamMemberModel> TeamMembers { get; set; } = new();
    public List<ProjectAssetAllocationModel> AssetAllocations { get; set; } = new();
}

public class TransitionStatusModel
{
    public ProjectStatus NewStatus { get; set; }
    public string? Remarks { get; set; }
}

public class DraftSessionModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid OrganizationId { get; set; }
    public string WizardStateJson { get; set; } = string.Empty;
    public int CurrentStep { get; set; } = 1;
    public DateTime LastSavedAtUtc { get; set; }
}
