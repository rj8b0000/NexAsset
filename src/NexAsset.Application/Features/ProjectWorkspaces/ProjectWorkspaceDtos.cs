using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.ProjectWorkspaces;

public sealed record ProjectCategoryDto(Guid Id, Guid OrganizationId, string Name, string? Description, bool IsSystemSuggested, bool IsActive);

public sealed record ProjectDto(
    Guid Id,
    Guid OrganizationId,
    Guid CategoryId,
    Guid? ClientId,
    Guid BranchId,
    Guid DepartmentId,
    Guid ProjectManagerId,
    string ProjectName,
    string? Description,
    ProjectPriority Priority,
    ProjectStatus Status,
    DateOnly StartDate,
    DateOnly? EndDate,
    DateOnly? ExpectedCompletion,
    string? Notes,
    bool IsArchived,
    DateTime? ArchivedAtUtc);

public sealed record ProjectMemberDto(
    Guid Id,
    Guid ProjectId,
    Guid EmployeeId,
    string RoleInProject,
    decimal AllocationPercentage,
    DateOnly JoinedOn,
    DateOnly? ReleasedOn,
    ProjectMemberStatus Status,
    string? Remarks);

public sealed record ProjectAssetAllocationDto(
    Guid Id,
    Guid ProjectId,
    Guid AssetId,
    int AllocatedQuantity,
    int ReturnedQuantity,
    DateOnly AllocatedOn,
    DateOnly? ReturnedOn,
    ProjectAssetAllocationStatus Status,
    string? Remarks);

public sealed record ProjectParameterGroupDto(Guid Id, Guid ProjectId, string GroupName, int DisplayOrder);

public sealed record ProjectParameterDto(
    Guid Id,
    Guid ProjectId,
    Guid GroupId,
    string ParameterName,
    ProjectParameterInputType InputType,
    string? Value,
    string? Unit,
    bool Required,
    int DisplayOrder,
    bool IsVisible);

public sealed record ProjectDocumentDto(
    Guid Id,
    Guid ProjectId,
    string Category,
    string DocumentName,
    string FilePath,
    Guid UploadedBy,
    DateTime UploadedOn,
    int Version,
    DateOnly? ExpiryDate,
    string? Remarks);

public sealed record ProjectActivityDto(Guid Id, Guid ProjectId, string ActivityType, string Message, Guid? ActorEmployeeId, DateTime OccurredAtUtc);

public sealed record ProjectDraftDto(
    Guid Id,
    Guid OrganizationId,
    Guid? ProjectId,
    Guid OwnerEmployeeId,
    int CurrentStep,
    string DraftName,
    string? DraftState,
    DateTime LastSavedAtUtc,
    bool IsSubmitted);

public sealed record ProjectBudgetDto(
    Guid Id,
    Guid ProjectId,
    decimal EstimatedBudget,
    decimal ActualCost,
    decimal RemainingBudget,
    decimal ProcurementCost,
    decimal MaintenanceCost,
    decimal LabourCost,
    decimal MiscellaneousCost);

public sealed record ProjectRiskDto(
    Guid Id,
    Guid ProjectId,
    string Title,
    string? Description,
    string Probability,
    string Impact,
    string Severity,
    string? MitigationPlan,
    Guid? OwnerEmployeeId,
    string Status,
    DateTime CreatedDate,
    DateTime? ClosedDate);

public sealed record ProjectSettingDto(
    Guid Id,
    Guid ProjectId,
    string Key,
    string Value,
    string? Description);

public sealed record ProjectDashboardKpiDto(
    int CompletionPercentage,
    string HealthStatus,
    int AssetsAllocated,
    int EmployeesAssigned,
    int ConsumablesUsed,
    int DocumentsUploaded,
    int OpenRisks,
    int PendingApprovals,
    int UpcomingDeadlines,
    int MaintenanceRequests,
    decimal BudgetUtilizationPercentage);
