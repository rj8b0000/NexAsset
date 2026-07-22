namespace NexAsset.Web.Models.Projects;

public sealed record ProjectListItem(Guid Id, string ProjectCode, string ProjectName, int Status, int Priority, Guid? ProjectManagerEmployeeId, DateOnly? StartDate, DateOnly? EndDate, DateTime CreatedAtUtc, string? ProjectManagerName, string? CategoryName);
public sealed record ProjectDetail(Guid Id, Guid OrganizationId, Guid? CustomerId, Guid CategoryId, Guid? BranchId, Guid? DepartmentId, Guid? ProjectManagerEmployeeId, string ProjectCode, string ProjectName, string? Description, string? Notes, string? InternalRemarks, int Status, int Priority, DateOnly? StartDate, DateOnly? EndDate, DateOnly? ExpectedCompletionDate, DateTime CreatedAtUtc, string? CustomerName, string? CategoryName, string? BranchName, string? DepartmentName, string? ProjectManagerName);
public sealed class ProjectInput
{
    public Guid OrganizationId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? ProjectManagerEmployeeId { get; set; }
    public string ProjectCode { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? InternalRemarks { get; set; }
    public int Priority { get; set; } = 2;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public DateOnly? ExpectedCompletionDate { get; set; }
}

public sealed record ProjectDashboard(ProjectDetail GeneralInfo, int ActiveTeamMembers, int ActiveAssets, int DocumentCount, int OpenRisks, BudgetSnapshot? Budget, IReadOnlyCollection<ActivityRecord> RecentActivities, bool HasUpcomingDeadline, int? DaysUntilDeadline, bool IsPendingApproval, int DaysElapsed, int? DaysRemaining);
public sealed record BudgetSnapshot(Guid Id, Guid ProjectId, decimal EstimatedBudget, decimal ApprovedBudget, decimal ActualCost, decimal ProcurementCost, decimal MaintenanceCost, decimal LabourCost, decimal MiscellaneousCost, DateTime CreatedAtUtc, decimal RemainingBudget, decimal BudgetPercentageUsed, string BudgetStatus);
public sealed record ActivityRecord(Guid Id, int ActivityType, string Action, Guid? UserId, string TargetEntity, Guid? TargetEntityId, DateTime Timestamp, string? Remarks, string? UserName);
public sealed record TeamMember(Guid Id, Guid ProjectId, Guid EmployeeId, string ProjectRole, int AllocationPercentage, DateOnly JoinedDate, DateOnly? ReleasedDate, int Status, string? Remarks, string? EmployeeName);
public sealed record AssetAllocation(Guid Id, Guid ProjectId, Guid AssetId, DateOnly AllocationDate, DateOnly? ReturnDate, int AllocatedQuantity, int ReturnedQuantity, int Status, string? Remarks, string? AssetCode, string? AssetName);
public sealed record ProjectDocument(Guid Id, Guid ProjectId, string DocumentName, int Category, string? Description, string FileReference, DateTime UploadedAtUtc, int Version, string? Remarks, DateOnly? ExpiryDate, Guid? UploadedByEmployeeId, string? UploadedByEmployeeName);
public sealed record ProjectRisk(Guid Id, Guid ProjectId, string Title, string? Description, int Category, int Probability, int Impact, int Severity, int Status, string? MitigationPlan, string? Remarks, DateTime? ClosedAtUtc, DateTime CreatedAtUtc, Guid? OwnerEmployeeId, string? OwnerName);
public sealed record ParameterSection(Guid Id, Guid ProjectId, string Name, int DisplayOrder, IReadOnlyCollection<ProjectParameter>? Parameters);
public sealed record ProjectParameter(Guid Id, string ParameterName, int InputType, string? Unit, bool IsRequired, int DisplayOrder, string? DropdownOptionsJson, string? Value);
public sealed record ProjectCategoryItem(Guid Id, Guid OrganizationId, string Name, string? Description, bool IsActive, DateTime CreatedAtUtc);
public sealed class ProjectCategoryInput
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

public sealed class DraftInput
{
    public Guid OrganizationId { get; set; }
    public string WizardStateJson { get; set; } = "{}";
    public int CurrentStep { get; set; } = 1;
}
