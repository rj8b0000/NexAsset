using FluentValidation;
using MediatR;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Projects;

public sealed record ProjectResponse(Guid Id, Guid OrganizationId, Guid? CustomerId, Guid CategoryId, Guid? BranchId, Guid? DepartmentId, Guid? ProjectManagerEmployeeId, string ProjectCode, string ProjectName, string? Description, string? Notes, string? InternalRemarks, ProjectStatus Status, ProjectPriority Priority, DateOnly? StartDate, DateOnly? EndDate, DateOnly? ExpectedCompletionDate, DateTime CreatedAtUtc, string? CustomerName = null, string? CategoryName = null, string? BranchName = null, string? DepartmentName = null, string? ProjectManagerName = null);
public sealed record ProjectListItemResponse(Guid Id, string ProjectCode, string ProjectName, ProjectStatus Status, ProjectPriority Priority, Guid? ProjectManagerEmployeeId, DateOnly? StartDate, DateOnly? EndDate, DateTime CreatedAtUtc, string? ProjectManagerName = null, string? CategoryName = null);
public sealed record DraftSessionResponse(Guid Id, Guid UserId, Guid OrganizationId, string WizardStateJson, int CurrentStep, DateTime LastSavedAtUtc);
public sealed record TeamMemberResponse(Guid Id, Guid ProjectId, Guid EmployeeId, string ProjectRole, int AllocationPercentage, DateOnly JoinedDate, DateOnly? ReleasedDate, TeamMemberStatus Status, string? Remarks, string? EmployeeName = null);
public sealed record AssetAllocationResponse(Guid Id, Guid ProjectId, Guid AssetId, DateOnly AllocationDate, DateOnly? ReturnDate, int AllocatedQuantity, int ReturnedQuantity, AllocationStatus Status, string? Remarks, string? AssetCode = null, string? AssetName = null);
public sealed record DocumentResponse(Guid Id, Guid ProjectId, string DocumentName, DocumentCategory Category, string? Description, string FileReference, DateTime UploadedAtUtc, int Version, string? Remarks, DateOnly? ExpiryDate, Guid? UploadedByEmployeeId, string? UploadedByEmployeeName = null);
public sealed record ParameterResponse(Guid Id, string ParameterName, ParameterInputType InputType, string? Unit, bool IsRequired, int DisplayOrder, string? DropdownOptionsJson, string? Value = null);
public sealed record ParameterSectionResponse(Guid Id, Guid ProjectId, string Name, int DisplayOrder, IReadOnlyList<ParameterResponse>? Parameters = null);
public sealed record BudgetResponse(Guid Id, Guid ProjectId, decimal EstimatedBudget, decimal ApprovedBudget, decimal ActualCost, decimal ProcurementCost, decimal MaintenanceCost, decimal LabourCost, decimal MiscellaneousCost, DateTime CreatedAtUtc, decimal RemainingBudget = 0, decimal BudgetPercentageUsed = 0, string BudgetStatus = "Under Budget");
public sealed record RiskResponse(Guid Id, Guid ProjectId, string Title, string? Description, RiskCategory Category, RiskProbability Probability, RiskImpact Impact, RiskSeverity Severity, RiskStatus Status, string? MitigationPlan, string? Remarks, DateTime? ClosedAtUtc, DateTime CreatedAtUtc, Guid? OwnerEmployeeId, string? OwnerName = null);
public sealed record TimelineEventResponse(Guid Id, TimelineEventType EventType, string Description, DateTime Timestamp, string EntityType, Guid? EntityId, Guid? UserId, string IconType, string? UserName = null);
public sealed record ActivityRecordResponse(Guid Id, ActivityType ActivityType, string Action, Guid? UserId, string TargetEntity, Guid? TargetEntityId, DateTime Timestamp, string? Remarks, string? UserName = null);
public sealed record SavedFilterResponse(Guid Id, string FilterName, string EntityType, string? SearchKeyword, string FilterCriteriaJson, Guid UserId, Guid OrganizationId);

public sealed record CreateProjectCommand(Guid OrganizationId, Guid CategoryId, Guid? CustomerId, Guid? BranchId, Guid? DepartmentId, Guid? ProjectManagerEmployeeId, string ProjectCode, string ProjectName, string? Description, string? Notes, string? InternalRemarks, ProjectPriority Priority, DateOnly? StartDate, DateOnly? EndDate, DateOnly? ExpectedCompletionDate) : IRequest<Result<ProjectResponse>>;
public sealed record UpdateProjectCommand(Guid Id, Guid OrganizationId, Guid CategoryId, Guid? CustomerId, Guid? BranchId, Guid? DepartmentId, Guid? ProjectManagerEmployeeId, string ProjectCode, string ProjectName, string? Description, string? Notes, string? InternalRemarks, ProjectPriority Priority, DateOnly? StartDate, DateOnly? EndDate, DateOnly? ExpectedCompletionDate) : IRequest<Result<ProjectResponse>>;
public sealed record UpdateProjectRequest(Guid OrganizationId, Guid CategoryId, Guid? CustomerId, Guid? BranchId, Guid? DepartmentId, Guid? ProjectManagerEmployeeId, string ProjectCode, string ProjectName, string? Description, string? Notes, string? InternalRemarks, ProjectPriority Priority, DateOnly? StartDate, DateOnly? EndDate, DateOnly? ExpectedCompletionDate);
public sealed record DeleteProjectCommand(Guid Id) : IRequest<Result>;
public sealed record TransitionProjectStatusCommand(Guid Id, ProjectStatus NewStatus) : IRequest<Result<ProjectResponse>>;
public sealed record DuplicateProjectCommand(Guid Id, string? ProjectCode = null) : IRequest<Result<ProjectResponse>>;
public sealed record GetProjectQuery(Guid Id) : IRequest<Result<ProjectResponse>>;
public sealed class GetProjectsQuery : PagedRequest, IRequest<Result<PagedResponse<ProjectListItemResponse>>>
{
    public Guid? OrganizationId { get; init; }
    public ProjectStatus? Status { get; init; }
    public ProjectPriority? Priority { get; init; }
    public Guid? CategoryId { get; init; }
    public Guid? BranchId { get; init; }
    public Guid? DepartmentId { get; init; }
    public Guid? ProjectManagerEmployeeId { get; init; }
}

public sealed record UpsertDraftSessionCommand(Guid OrganizationId, string WizardStateJson, int CurrentStep) : IRequest<Result<DraftSessionResponse>>;
public sealed record GetDraftSessionQuery(Guid OrganizationId) : IRequest<Result<DraftSessionResponse>>;
public sealed record DeleteDraftSessionCommand(Guid OrganizationId) : IRequest<Result>;

public sealed record AddTeamMemberCommand(Guid ProjectId, Guid EmployeeId, string ProjectRole, int AllocationPercentage, DateOnly JoinedDate, DateOnly? ReleasedDate, string? Remarks) : IRequest<Result<TeamMemberResponse>>;
public sealed record UpdateTeamMemberCommand(Guid ProjectId, Guid Id, string ProjectRole, int AllocationPercentage, string? Remarks) : IRequest<Result<TeamMemberResponse>>;
public sealed record ReleaseTeamMemberCommand(Guid ProjectId, Guid Id, DateOnly ReleasedDate, string? Remarks) : IRequest<Result<TeamMemberResponse>>;
public sealed record RemoveTeamMemberCommand(Guid ProjectId, Guid Id) : IRequest<Result>;
public sealed class GetTeamMembersQuery : PagedRequest, IRequest<Result<PagedResponse<TeamMemberResponse>>>
{
    public Guid ProjectId { get; init; }
    public TeamMemberStatus? Status { get; init; }
}

public sealed record AllocateAssetCommand(Guid ProjectId, Guid AssetId, DateOnly AllocationDate, int AllocatedQuantity, string? Remarks) : IRequest<Result<AssetAllocationResponse>>;
public sealed record ReturnAssetCommand(Guid ProjectId, Guid Id, DateOnly ReturnDate, int ReturnedQuantity, bool IsAssetUsable, string? Remarks) : IRequest<Result<AssetAllocationResponse>>;
public sealed class GetAssetAllocationsQuery : PagedRequest, IRequest<Result<PagedResponse<AssetAllocationResponse>>>
{
    public Guid ProjectId { get; init; }
    public AllocationStatus? Status { get; init; }
}

public sealed record UploadDocumentCommand(Guid ProjectId, string DocumentName, DocumentCategory Category, string? Description, string? Remarks, DateOnly? ExpiryDate, string FileName, string ContentType, string FileContentBase64) : IRequest<Result<DocumentResponse>>;
public sealed record ReplaceDocumentCommand(Guid ProjectId, Guid Id, string FileName, string ContentType, string FileContentBase64, string? Remarks) : IRequest<Result<DocumentResponse>>;
public sealed record DeleteDocumentCommand(Guid ProjectId, Guid Id) : IRequest<Result>;
public sealed class GetDocumentsQuery : PagedRequest, IRequest<Result<PagedResponse<DocumentResponse>>>
{
    public Guid ProjectId { get; init; }
    public DocumentCategory? Category { get; init; }
}
public sealed record GetDocumentVersionHistoryQuery(Guid ProjectId, string DocumentName) : IRequest<Result<IReadOnlyCollection<DocumentResponse>>>;

public sealed record CreateParameterSectionCommand(Guid ProjectId, string Name, int DisplayOrder) : IRequest<Result<ParameterSectionResponse>>;
public sealed record UpdateParameterSectionCommand(Guid ProjectId, Guid Id, string Name, int DisplayOrder) : IRequest<Result<ParameterSectionResponse>>;
public sealed record DeleteParameterSectionCommand(Guid ProjectId, Guid Id) : IRequest<Result>;
public sealed record AddParameterCommand(Guid ProjectId, Guid SectionId, string ParameterName, ParameterInputType InputType, string? Unit, bool IsRequired, int DisplayOrder, string? DropdownOptionsJson) : IRequest<Result<ParameterResponse>>;
public sealed record UpdateParameterCommand(Guid ProjectId, Guid SectionId, Guid Id, string ParameterName, ParameterInputType InputType, string? Unit, bool IsRequired, int DisplayOrder, string? DropdownOptionsJson) : IRequest<Result<ParameterResponse>>;
public sealed record DeleteParameterCommand(Guid ProjectId, Guid SectionId, Guid Id) : IRequest<Result>;
public sealed record ParameterValueInput(Guid ParameterId, string? Value);
public sealed record SaveParameterValuesCommand(Guid ProjectId, IReadOnlyCollection<ParameterValueInput> Values) : IRequest<Result>;
public sealed record GetParameterSectionsQuery(Guid ProjectId) : IRequest<Result<IReadOnlyCollection<ParameterSectionResponse>>>;

public sealed record UpdateBudgetCommand(Guid ProjectId, decimal EstimatedBudget, decimal ApprovedBudget, decimal ActualCost, decimal ProcurementCost, decimal MaintenanceCost, decimal LabourCost, decimal MiscellaneousCost) : IRequest<Result<BudgetResponse>>;
public sealed record GetCurrentBudgetQuery(Guid ProjectId) : IRequest<Result<BudgetResponse>>;
public sealed class GetBudgetHistoryQuery : PagedRequest, IRequest<Result<PagedResponse<BudgetResponse>>>
{
    public Guid ProjectId { get; init; }
}

public sealed record CreateRiskCommand(Guid ProjectId, string Title, string? Description, RiskCategory Category, RiskProbability Probability, RiskImpact Impact, Guid? OwnerEmployeeId, string? MitigationPlan, string? Remarks) : IRequest<Result<RiskResponse>>;
public sealed record UpdateRiskCommand(Guid ProjectId, Guid Id, string Title, string? Description, RiskCategory Category, RiskProbability Probability, RiskImpact Impact, RiskStatus Status, Guid? OwnerEmployeeId, string? MitigationPlan, string? Remarks) : IRequest<Result<RiskResponse>>;
public sealed record CloseRiskCommand(Guid ProjectId, Guid Id, string? Remarks) : IRequest<Result<RiskResponse>>;
public sealed record DeleteRiskCommand(Guid ProjectId, Guid Id) : IRequest<Result>;
public sealed record GetRiskQuery(Guid ProjectId, Guid Id) : IRequest<Result<RiskResponse>>;
public sealed class GetRisksQuery : PagedRequest, IRequest<Result<PagedResponse<RiskResponse>>>
{
    public Guid ProjectId { get; init; }
    public RiskCategory? Category { get; init; }
    public RiskProbability? Probability { get; init; }
    public RiskImpact? Impact { get; init; }
    public RiskSeverity? Severity { get; init; }
    public RiskStatus? Status { get; init; }
    public Guid? OwnerEmployeeId { get; init; }
}

public sealed class GetTimelineQuery : PagedRequest, IRequest<Result<PagedResponse<TimelineEventResponse>>>
{
    public Guid ProjectId { get; init; }
    public TimelineEventType? EventType { get; init; }
    public string? Keyword { get; init; }
}
public sealed class GetActivitiesQuery : PagedRequest, IRequest<Result<PagedResponse<ActivityRecordResponse>>>
{
    public Guid ProjectId { get; init; }
    public ActivityType? ActivityType { get; init; }
    public Guid? UserId { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public string? Keyword { get; init; }
}

public sealed record ProjectDashboardResponse(ProjectResponse GeneralInfo, int ActiveTeamMembers, int ActiveAssets, int DocumentCount, int OpenRisks, BudgetResponse? Budget, IReadOnlyCollection<ActivityRecordResponse> RecentActivities, bool HasUpcomingDeadline, int? DaysUntilDeadline, bool IsPendingApproval, int DaysElapsed, int? DaysRemaining);
public sealed record GetProjectDashboardQuery(Guid ProjectId) : IRequest<Result<ProjectDashboardResponse>>;

public sealed record ProjectSummaryReportResponse(ProjectResponse Project);
public sealed record TeamAllocationReportResponse(IReadOnlyCollection<TeamMemberResponse> Items);
public sealed record AssetAllocationReportResponse(IReadOnlyCollection<AssetAllocationResponse> Items);
public sealed record BudgetReportResponse(IReadOnlyCollection<BudgetResponse> Items);
public sealed record RiskReportResponse(IReadOnlyCollection<RiskResponse> Items);
public sealed record DocumentRegisterReportResponse(IReadOnlyCollection<DocumentResponse> Items);
public sealed record ActivityReportResponse(IReadOnlyCollection<ActivityRecordResponse> Items);
public sealed record TimelineReportResponse(IReadOnlyCollection<TimelineEventResponse> Items);
public sealed record ParameterReportResponse(IReadOnlyCollection<ParameterSectionResponse> Items);
public sealed record GetProjectSummaryReportQuery(Guid ProjectId) : IRequest<Result<ProjectSummaryReportResponse>>;
public sealed record GetTeamAllocationReportQuery(Guid ProjectId) : IRequest<Result<TeamAllocationReportResponse>>;
public sealed record GetAssetAllocationReportQuery(Guid ProjectId) : IRequest<Result<AssetAllocationReportResponse>>;
public sealed record GetBudgetReportQuery(Guid ProjectId) : IRequest<Result<BudgetReportResponse>>;
public sealed record GetRiskReportQuery(Guid ProjectId) : IRequest<Result<RiskReportResponse>>;
public sealed record GetDocumentRegisterReportQuery(Guid ProjectId) : IRequest<Result<DocumentRegisterReportResponse>>;
public sealed record GetActivityReportQuery(Guid ProjectId, ActivityType? ActivityType = null, Guid? UserId = null, DateTime? FromDate = null, DateTime? ToDate = null) : IRequest<Result<ActivityReportResponse>>;
public sealed record GetTimelineReportQuery(Guid ProjectId, TimelineEventType? EventType = null, DateTime? FromDate = null, DateTime? ToDate = null) : IRequest<Result<TimelineReportResponse>>;
public sealed record GetParameterReportQuery(Guid ProjectId) : IRequest<Result<ParameterReportResponse>>;
public enum ProjectReportType { Summary, Team, Assets, Budget, Risks, Documents, Activities, Timeline, Parameters }
public sealed record ExportReportPdfCommand(Guid ProjectId, ProjectReportType ReportType) : IRequest<Result<ReportExportResponse>>;
public sealed record ExportReportExcelCommand(Guid ProjectId, ProjectReportType ReportType) : IRequest<Result<ReportExportResponse>>;
public sealed record ReportExportResponse(string FileName, string ContentType, byte[] Content);

public sealed record SearchResultItem(Guid Id, string DisplayText, string Highlight, string NavigationUrl);
public sealed record SearchResultGroup(string EntityType, int MatchCount, IReadOnlyCollection<SearchResultItem> Items);
public sealed record GlobalSearchResponse(IReadOnlyCollection<SearchResultGroup> Groups, int TotalCount, int PageNumber, int PageSize);
public sealed class GlobalSearchQuery : PagedRequest, IRequest<Result<GlobalSearchResponse>>
{
    public Guid? OrganizationId { get; init; }
    public string Keyword { get; init; } = string.Empty;
}
public sealed record SaveFilterCommand(Guid OrganizationId, string FilterName, string EntityType, string? SearchKeyword, string FilterCriteriaJson) : IRequest<Result<SavedFilterResponse>>;
public sealed record GetSavedFiltersQuery(Guid OrganizationId, string? EntityType = null) : IRequest<Result<IReadOnlyCollection<SavedFilterResponse>>>;
public sealed record DeleteSavedFilterCommand(Guid Id) : IRequest<Result>;

public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.ProjectCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ProjectName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Notes).MaximumLength(2000);
        RuleFor(x => x.InternalRemarks).MaximumLength(2000);
        RuleFor(x => x).Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.StartDate <= x.EndDate).WithMessage("Start date must not be after end date.");
    }
}

public sealed class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.ProjectCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ProjectName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Notes).MaximumLength(2000);
        RuleFor(x => x.InternalRemarks).MaximumLength(2000);
        RuleFor(x => x).Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.StartDate <= x.EndDate).WithMessage("Start date must not be after end date.");
    }
}

public sealed class TransitionProjectStatusCommandValidator : AbstractValidator<TransitionProjectStatusCommand>
{
    public TransitionProjectStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NewStatus).IsInEnum();
    }
}

public sealed class AddTeamMemberCommandValidator : AbstractValidator<AddTeamMemberCommand>
{
    public AddTeamMemberCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.ProjectRole).NotEmpty().MaximumLength(100);
        RuleFor(x => x.AllocationPercentage).InclusiveBetween(1, 100);
        RuleFor(x => x).Must(x => !x.ReleasedDate.HasValue || x.JoinedDate <= x.ReleasedDate).WithMessage("Joined date must not be after released date.");
    }
}

public sealed class AllocateAssetCommandValidator : AbstractValidator<AllocateAssetCommand>
{
    public AllocateAssetCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.AllocatedQuantity).GreaterThan(0);
    }
}

public sealed class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
{
    public UploadDocumentCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.DocumentName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Remarks).MaximumLength(500);
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.FileContentBase64).NotEmpty();
    }
}

public sealed class AddParameterCommandValidator : AbstractValidator<AddParameterCommand>
{
    public AddParameterCommandValidator()
    {
        RuleFor(x => x.ParameterName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.InputType).IsInEnum();
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DropdownOptionsJson).NotEmpty().When(x => x.InputType == ParameterInputType.Dropdown);
    }
}

public sealed class UpdateBudgetCommandValidator : AbstractValidator<UpdateBudgetCommand>
{
    public UpdateBudgetCommandValidator()
    {
        RuleFor(x => x.EstimatedBudget).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ApprovedBudget).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ActualCost).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ProcurementCost).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaintenanceCost).GreaterThanOrEqualTo(0);
        RuleFor(x => x.LabourCost).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MiscellaneousCost).GreaterThanOrEqualTo(0);
    }
}

public sealed class CreateRiskCommandValidator : AbstractValidator<CreateRiskCommand>
{
    public CreateRiskCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.MitigationPlan).MaximumLength(1000);
        RuleFor(x => x.Remarks).MaximumLength(500);
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.Probability).IsInEnum();
        RuleFor(x => x.Impact).IsInEnum();
    }
}
