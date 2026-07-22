using System.IO.Compression;
using System.Security;
using System.Text;
using System.Xml;
using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;
using NexAsset.Domain.Helpers;

namespace NexAsset.Application.Features.Projects;

public sealed class ProjectWorkspaceInsightHandlers(
    IProjectRepository projects,
    IProjectBudgetRepository budgets,
    IProjectRiskRepository risks,
    IProjectTeamRepository teams,
    IProjectAssetRepository allocations,
    IProjectDocumentRepository documents,
    IProjectParameterRepository parameters,
    IProjectTimelineRepository timeline,
    IProjectActivityRepository activities,
    IEmployeeRepository employees,
    IEnterpriseOperationsRepository operations,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateBudgetCommand, Result<BudgetResponse>>,
      IRequestHandler<GetCurrentBudgetQuery, Result<BudgetResponse>>,
      IRequestHandler<GetBudgetHistoryQuery, Result<PagedResponse<BudgetResponse>>>,
      IRequestHandler<CreateRiskCommand, Result<RiskResponse>>,
      IRequestHandler<UpdateRiskCommand, Result<RiskResponse>>,
      IRequestHandler<CloseRiskCommand, Result<RiskResponse>>,
      IRequestHandler<DeleteRiskCommand, Result>,
      IRequestHandler<GetRisksQuery, Result<PagedResponse<RiskResponse>>>,
      IRequestHandler<GetRiskQuery, Result<RiskResponse>>,
      IRequestHandler<GetTimelineQuery, Result<PagedResponse<TimelineEventResponse>>>,
      IRequestHandler<GetActivitiesQuery, Result<PagedResponse<ActivityRecordResponse>>>,
      IRequestHandler<GetProjectDashboardQuery, Result<ProjectDashboardResponse>>,
      IRequestHandler<GetProjectSummaryReportQuery, Result<ProjectSummaryReportResponse>>,
      IRequestHandler<GetTeamAllocationReportQuery, Result<TeamAllocationReportResponse>>,
      IRequestHandler<GetAssetAllocationReportQuery, Result<AssetAllocationReportResponse>>,
      IRequestHandler<GetBudgetReportQuery, Result<BudgetReportResponse>>,
      IRequestHandler<GetRiskReportQuery, Result<RiskReportResponse>>,
      IRequestHandler<GetDocumentRegisterReportQuery, Result<DocumentRegisterReportResponse>>,
      IRequestHandler<GetActivityReportQuery, Result<ActivityReportResponse>>,
      IRequestHandler<GetTimelineReportQuery, Result<TimelineReportResponse>>,
      IRequestHandler<GetParameterReportQuery, Result<ParameterReportResponse>>,
      IRequestHandler<ExportReportPdfCommand, Result<ReportExportResponse>>,
      IRequestHandler<ExportReportExcelCommand, Result<ReportExportResponse>>
{
    public async Task<Result<BudgetResponse>> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
    {
        var project = await projects.GetByIdWithDetailsAsync(request.ProjectId, cancellationToken);
        if (project is null) return Result<BudgetResponse>.Failure("Project not found.");
        var budget = ProjectWorkspaceMapper.ToEntity(request);
        budget.UpdatedByUserId = currentUser.UserId ?? Guid.Empty;
        await budgets.AddAsync(budget, cancellationToken);
        await RecordAsync(project.Id, TimelineEventType.BudgetUpdated, ActivityType.BudgetUpdated, "Project budget updated.", "ProjectBudget", budget.Id, cancellationToken);
        var response = ToResponse(budget);
        if (response.BudgetPercentageUsed >= 80)
            await NotifyManagerAsync(project.ProjectManagerEmployeeId, "Project budget threshold", $"Project {project.ProjectName} has used {response.BudgetPercentageUsed:N1}% of its approved budget.", cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<BudgetResponse>.Success(response);
    }

    public async Task<Result<BudgetResponse>> Handle(GetCurrentBudgetQuery request, CancellationToken cancellationToken)
    {
        if (await projects.GetByIdAsync(request.ProjectId, cancellationToken) is null) return Result<BudgetResponse>.Failure("Project not found.");
        var budget = await budgets.GetLatestByProjectAsync(request.ProjectId, cancellationToken);
        return budget is null ? Result<BudgetResponse>.Failure("No budget has been recorded for this project.") : Result<BudgetResponse>.Success(ToResponse(budget));
    }

    public async Task<Result<PagedResponse<BudgetResponse>>> Handle(GetBudgetHistoryQuery request, CancellationToken cancellationToken)
    {
        var page = await budgets.GetHistoryAsync(request.ProjectId, request, cancellationToken);
        return Result<PagedResponse<BudgetResponse>>.Success(page.Map(ToResponse));
    }

    public async Task<Result<RiskResponse>> Handle(CreateRiskCommand request, CancellationToken cancellationToken)
    {
        var project = await projects.GetByIdWithDetailsAsync(request.ProjectId, cancellationToken);
        if (project is null) return Result<RiskResponse>.Failure("Project not found.");
        Employee? owner = null;
        if (request.OwnerEmployeeId.HasValue)
        {
            owner = await employees.GetByIdAsync(request.OwnerEmployeeId.Value, cancellationToken);
            if (owner is null || owner.OrganizationId != project.OrganizationId) return Result<RiskResponse>.Failure("Risk owner not found for this project organization.");
        }
        var risk = ProjectWorkspaceMapper.ToEntity(request);
        risk.Severity = RiskSeverityHelper.ComputeSeverity(request.Probability, request.Impact);
        risk.Status = RiskStatus.Open;
        await risks.AddAsync(risk, cancellationToken);
        await RecordAsync(project.Id, TimelineEventType.RiskAdded, ActivityType.RiskAdded, $"Risk {risk.Title} added.", "ProjectRisk", risk.Id, cancellationToken);
        await NotifyManagerAsync(project.ProjectManagerEmployeeId, "Project risk created", $"Risk {risk.Title} was added to {project.ProjectName}.", cancellationToken);
        if (risk.Severity == RiskSeverity.Critical && owner is not null) await NotifyEmployeeAsync(owner, "Critical project risk", $"You are the owner of critical risk {risk.Title}.", cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<RiskResponse>.Success(ToResponse(risk, owner));
    }

    public async Task<Result<RiskResponse>> Handle(UpdateRiskCommand request, CancellationToken cancellationToken)
    {
        var risk = await risks.GetByIdAsync(request.Id, cancellationToken);
        if (risk is null || risk.ProjectId != request.ProjectId) return Result<RiskResponse>.Failure("Project risk not found.");
        var project = await projects.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null) return Result<RiskResponse>.Failure("Project not found.");
        Employee? owner = null;
        if (request.OwnerEmployeeId.HasValue)
        {
            owner = await employees.GetByIdAsync(request.OwnerEmployeeId.Value, cancellationToken);
            if (owner is null || owner.OrganizationId != project.OrganizationId) return Result<RiskResponse>.Failure("Risk owner not found for this project organization.");
        }
        risk.Title = request.Title;
        risk.Description = request.Description;
        risk.Category = request.Category;
        risk.Probability = request.Probability;
        risk.Impact = request.Impact;
        risk.Severity = RiskSeverityHelper.ComputeSeverity(request.Probability, request.Impact);
        risk.Status = request.Status;
        risk.OwnerEmployeeId = request.OwnerEmployeeId;
        risk.OwnerEmployee = owner;
        risk.MitigationPlan = request.MitigationPlan;
        risk.Remarks = request.Remarks;
        risk.UpdatedAtUtc = DateTime.UtcNow;
        risks.Update(risk);
        await RecordAsync(request.ProjectId, null, ActivityType.RiskUpdated, $"Risk {risk.Title} updated.", "ProjectRisk", risk.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<RiskResponse>.Success(ToResponse(risk, owner));
    }

    public async Task<Result<RiskResponse>> Handle(CloseRiskCommand request, CancellationToken cancellationToken)
    {
        var risk = await risks.GetByIdAsync(request.Id, cancellationToken);
        if (risk is null || risk.ProjectId != request.ProjectId) return Result<RiskResponse>.Failure("Project risk not found.");
        risk.Status = RiskStatus.Closed;
        risk.ClosedAtUtc = DateTime.UtcNow;
        risk.Remarks = request.Remarks ?? risk.Remarks;
        risk.UpdatedAtUtc = DateTime.UtcNow;
        risks.Update(risk);
        await RecordAsync(request.ProjectId, TimelineEventType.RiskClosed, ActivityType.RiskClosed, $"Risk {risk.Title} closed.", "ProjectRisk", risk.Id, cancellationToken);
        var project = await projects.GetByIdWithDetailsAsync(request.ProjectId, cancellationToken);
        if (project is not null) await NotifyManagerAsync(project.ProjectManagerEmployeeId, "Project risk closed", $"Risk {risk.Title} was closed.", cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<RiskResponse>.Success(ToResponse(risk, risk.OwnerEmployee));
    }

    public async Task<Result> Handle(DeleteRiskCommand request, CancellationToken cancellationToken)
    {
        var risk = await risks.GetByIdAsync(request.Id, cancellationToken);
        if (risk is null || risk.ProjectId != request.ProjectId) return Result.Failure("Project risk not found.");
        risk.IsDeleted = true;
        risk.DeletedAtUtc = DateTime.UtcNow;
        risk.UpdatedAtUtc = DateTime.UtcNow;
        risks.Remove(risk);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<PagedResponse<RiskResponse>>> Handle(GetRisksQuery request, CancellationToken cancellationToken)
    {
        var page = await risks.GetPagedAsync(request, cancellationToken);
        return Result<PagedResponse<RiskResponse>>.Success(page.Map(x => ToResponse(x, x.OwnerEmployee)));
    }

    public async Task<Result<RiskResponse>> Handle(GetRiskQuery request, CancellationToken cancellationToken)
    {
        var risk = await risks.GetByIdAsync(request.Id, cancellationToken);
        return risk is null || risk.ProjectId != request.ProjectId
            ? Result<RiskResponse>.Failure("Project risk not found.")
            : Result<RiskResponse>.Success(ToResponse(risk, risk.OwnerEmployee));
    }

    public async Task<Result<PagedResponse<TimelineEventResponse>>> Handle(GetTimelineQuery request, CancellationToken cancellationToken)
    {
        var page = await timeline.GetPagedAsync(request.ProjectId, request.EventType, request.Keyword, request.PageNumber, request.PageSize, cancellationToken);
        return Result<PagedResponse<TimelineEventResponse>>.Success(page.Map(ToResponse));
    }

    public async Task<Result<PagedResponse<ActivityRecordResponse>>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
    {
        var page = await activities.GetPagedAsync(request, cancellationToken);
        return Result<PagedResponse<ActivityRecordResponse>>.Success(page.Map(ToResponse));
    }

    public async Task<Result<ProjectDashboardResponse>> Handle(GetProjectDashboardQuery request, CancellationToken cancellationToken)
    {
        var project = await projects.GetByIdWithDetailsAsync(request.ProjectId, cancellationToken);
        if (project is null) return Result<ProjectDashboardResponse>.Failure("Project not found.");
        var allTeam = await teams.GetAllAsync(request.ProjectId, cancellationToken);
        var allAssets = await allocations.GetAllAsync(request.ProjectId, cancellationToken);
        var allDocuments = await documents.GetAllAsync(request.ProjectId, cancellationToken);
        var allRisks = await risks.GetAllAsync(request.ProjectId, cancellationToken);
        var budget = await budgets.GetLatestByProjectAsync(request.ProjectId, cancellationToken);
        var recent = await activities.GetRecentAsync(request.ProjectId, 10, cancellationToken);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        int? daysUntil = project.ExpectedCompletionDate.HasValue ? project.ExpectedCompletionDate.Value.DayNumber - today.DayNumber : null;
        var daysElapsed = project.StartDate.HasValue ? Math.Max(0, today.DayNumber - project.StartDate.Value.DayNumber) : 0;
        return Result<ProjectDashboardResponse>.Success(new ProjectDashboardResponse(
            ToProjectResponse(project),
            allTeam.Count(x => x.Status == TeamMemberStatus.Active),
            allAssets.Count(x => x.Status != AllocationStatus.Returned),
            allDocuments.Count,
            allRisks.Count(x => x.Status != RiskStatus.Closed),
            budget is null ? null : ToResponse(budget),
            recent.Select(ToResponse).ToList(),
            daysUntil is >= 0 and <= 30,
            daysUntil,
            project.Status == ProjectStatus.AwaitingApproval,
            daysElapsed,
            daysUntil));
    }

    public async Task<Result<ProjectSummaryReportResponse>> Handle(GetProjectSummaryReportQuery request, CancellationToken cancellationToken)
    {
        var project = await projects.GetByIdWithDetailsAsync(request.ProjectId, cancellationToken);
        return project is null ? Result<ProjectSummaryReportResponse>.Failure("Project not found.") : Result<ProjectSummaryReportResponse>.Success(new ProjectSummaryReportResponse(ToProjectResponse(project)));
    }

    public async Task<Result<TeamAllocationReportResponse>> Handle(GetTeamAllocationReportQuery request, CancellationToken cancellationToken) =>
        Result<TeamAllocationReportResponse>.Success(new TeamAllocationReportResponse((await teams.GetAllAsync(request.ProjectId, cancellationToken)).Select(x => ToResponse(x, x.Employee)).ToList()));

    public async Task<Result<AssetAllocationReportResponse>> Handle(GetAssetAllocationReportQuery request, CancellationToken cancellationToken) =>
        Result<AssetAllocationReportResponse>.Success(new AssetAllocationReportResponse((await allocations.GetAllAsync(request.ProjectId, cancellationToken)).Select(x => ToResponse(x, x.Asset)).ToList()));

    public async Task<Result<BudgetReportResponse>> Handle(GetBudgetReportQuery request, CancellationToken cancellationToken)
    {
        var page = await budgets.GetHistoryAsync(request.ProjectId, new PagedRequest { PageNumber = 1, PageSize = 200 }, cancellationToken);
        return Result<BudgetReportResponse>.Success(new BudgetReportResponse(page.Items.Select(ToResponse).ToList()));
    }

    public async Task<Result<RiskReportResponse>> Handle(GetRiskReportQuery request, CancellationToken cancellationToken) =>
        Result<RiskReportResponse>.Success(new RiskReportResponse((await risks.GetAllAsync(request.ProjectId, cancellationToken)).Select(x => ToResponse(x, x.OwnerEmployee)).ToList()));

    public async Task<Result<DocumentRegisterReportResponse>> Handle(GetDocumentRegisterReportQuery request, CancellationToken cancellationToken) =>
        Result<DocumentRegisterReportResponse>.Success(new DocumentRegisterReportResponse((await documents.GetAllAsync(request.ProjectId, cancellationToken)).Select(x => ToResponse(x, x.UploadedByEmployee)).ToList()));

    public async Task<Result<ActivityReportResponse>> Handle(GetActivityReportQuery request, CancellationToken cancellationToken)
    {
        var page = await activities.GetPagedAsync(new GetActivitiesQuery { ProjectId = request.ProjectId, ActivityType = request.ActivityType, UserId = request.UserId, From = request.FromDate, To = request.ToDate, PageNumber = 1, PageSize = 200 }, cancellationToken);
        return Result<ActivityReportResponse>.Success(new ActivityReportResponse(page.Items.Select(ToResponse).ToList()));
    }

    public async Task<Result<TimelineReportResponse>> Handle(GetTimelineReportQuery request, CancellationToken cancellationToken)
    {
        var items = await timeline.GetAllAsync(request.ProjectId, cancellationToken);
        var filtered = items.Where(x => (!request.EventType.HasValue || x.EventType == request.EventType.Value) && (!request.FromDate.HasValue || x.Timestamp >= request.FromDate.Value) && (!request.ToDate.HasValue || x.Timestamp <= request.ToDate.Value)).Select(ToResponse).ToList();
        return Result<TimelineReportResponse>.Success(new TimelineReportResponse(filtered));
    }

    public async Task<Result<ParameterReportResponse>> Handle(GetParameterReportQuery request, CancellationToken cancellationToken)
    {
        var sections = await parameters.GetSectionsAsync(request.ProjectId, cancellationToken);
        var values = await parameters.GetValuesByProjectAsync(request.ProjectId, cancellationToken);
        var byParameter = values.ToDictionary(x => x.ParameterId, x => x.Value);
        var response = sections.Select(section => ProjectWorkspaceMapper.ToResponse(section) with { Parameters = section.Parameters.OrderBy(x => x.DisplayOrder).Select(x => ProjectWorkspaceMapper.ToResponse(x) with { Value = byParameter.GetValueOrDefault(x.Id) }).ToList() }).ToList();
        return Result<ParameterReportResponse>.Success(new ParameterReportResponse(response));
    }

    public async Task<Result<ReportExportResponse>> Handle(ExportReportPdfCommand request, CancellationToken cancellationToken)
    {
        var text = await BuildReportTextAsync(request.ProjectId, request.ReportType, cancellationToken);
        return text is null
            ? Result<ReportExportResponse>.Failure("Project not found.")
            : Result<ReportExportResponse>.Success(new ReportExportResponse($"project-{request.ProjectId:N}-{request.ReportType}.pdf", "application/pdf", ProjectWorkspaceExport.BuildPdf(text)));
    }

    public async Task<Result<ReportExportResponse>> Handle(ExportReportExcelCommand request, CancellationToken cancellationToken)
    {
        var text = await BuildReportTextAsync(request.ProjectId, request.ReportType, cancellationToken);
        return text is null
            ? Result<ReportExportResponse>.Failure("Project not found.")
            : Result<ReportExportResponse>.Success(new ReportExportResponse($"project-{request.ProjectId:N}-{request.ReportType}.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ProjectWorkspaceExport.BuildExcel(text)));
    }

    private async Task<string?> BuildReportTextAsync(Guid projectId, ProjectReportType type, CancellationToken cancellationToken)
    {
        var project = await projects.GetByIdWithDetailsAsync(projectId, cancellationToken);
        if (project is null) return null;
        var lines = new List<string> { "NexAsset Project Report", $"Project: {project.ProjectCode} - {project.ProjectName}", $"Report: {type}", $"Generated (UTC): {DateTime.UtcNow:O}", string.Empty };
        switch (type)
        {
            case ProjectReportType.Team:
                lines.AddRange((await teams.GetAllAsync(projectId, cancellationToken)).Select(x => $"{x.Employee.FirstName} {x.Employee.LastName} | {x.ProjectRole} | {x.AllocationPercentage}% | {x.Status}"));
                break;
            case ProjectReportType.Assets:
                lines.AddRange((await allocations.GetAllAsync(projectId, cancellationToken)).Select(x => $"{x.Asset.AssetCode} | {x.Asset.AssetName} | {x.AllocatedQuantity} | {x.Status}"));
                break;
            case ProjectReportType.Budget:
                lines.AddRange((await budgets.GetHistoryAsync(projectId, new PagedRequest { PageNumber = 1, PageSize = 200 }, cancellationToken)).Items.Select(x => $"{x.CreatedAtUtc:yyyy-MM-dd} | Approved {x.ApprovedBudget:N2} | Actual {x.ActualCost:N2}"));
                break;
            case ProjectReportType.Risks:
                lines.AddRange((await risks.GetAllAsync(projectId, cancellationToken)).Select(x => $"{x.Title} | {x.Severity} | {x.Status}"));
                break;
            case ProjectReportType.Documents:
                lines.AddRange((await documents.GetAllAsync(projectId, cancellationToken)).Select(x => $"{x.DocumentName} | {x.Category} | v{x.Version}"));
                break;
            case ProjectReportType.Activities:
                lines.AddRange((await activities.GetRecentAsync(projectId, 200, cancellationToken)).Select(x => $"{x.Timestamp:O} | {x.Action}"));
                break;
            case ProjectReportType.Timeline:
                lines.AddRange((await timeline.GetAllAsync(projectId, cancellationToken)).Select(x => $"{x.Timestamp:O} | {x.Description}"));
                break;
            case ProjectReportType.Parameters:
                lines.AddRange((await parameters.GetSectionsAsync(projectId, cancellationToken)).SelectMany(x => x.Parameters.Select(p => $"{x.Name} | {p.ParameterName} | {p.InputType}")));
                break;
            default:
                lines.Add($"Status: {project.Status}");
                lines.Add($"Priority: {project.Priority}");
                lines.Add($"Start: {project.StartDate}");
                lines.Add($"Expected completion: {project.ExpectedCompletionDate}");
                break;
        }
        return string.Join(Environment.NewLine, lines);
    }

    private async Task RecordAsync(Guid projectId, TimelineEventType? timelineType, ActivityType activityType, string action, string entityType, Guid? entityId, CancellationToken cancellationToken)
    {
        if (timelineType.HasValue) await timeline.AddAsync(new ProjectTimelineEvent { ProjectId = projectId, EventType = timelineType.Value, EntityType = entityType, EntityId = entityId, Description = action, Timestamp = DateTime.UtcNow, UserId = currentUser.UserId, IconType = timelineType.Value.ToString() }, cancellationToken);
        await activities.AddAsync(new ProjectActivityRecord { ProjectId = projectId, ActivityType = activityType, Action = action, TargetEntity = entityType, TargetEntityId = entityId, Timestamp = DateTime.UtcNow, UserId = currentUser.UserId }, cancellationToken);
        await operations.AddAsync(new AuditLog { UserId = currentUser.UserId, EntityName = entityType, EntityId = entityId, Action = action }, cancellationToken);
    }

    private async Task NotifyManagerAsync(Guid? employeeId, string title, string message, CancellationToken cancellationToken)
    {
        if (!employeeId.HasValue) return;
        var employee = await employees.GetByIdAsync(employeeId.Value, cancellationToken);
        if (employee is not null) await NotifyEmployeeAsync(employee, title, message, cancellationToken);
    }

    private Task NotifyEmployeeAsync(Employee employee, string title, string message, CancellationToken cancellationToken) => employee.IdentityUserId.HasValue
        ? operations.AddAsync(new Notification { UserId = employee.IdentityUserId, Title = title, Message = message, NotificationType = NotificationType.Info }, cancellationToken)
        : Task.CompletedTask;

    private static BudgetResponse ToResponse(ProjectBudget budget)
    {
        var percentage = budget.ApprovedBudget == 0 ? 0 : Math.Round(budget.ActualCost / budget.ApprovedBudget * 100, 2);
        var status = budget.ActualCost > budget.ApprovedBudget ? "Over Budget" : budget.ActualCost == budget.ApprovedBudget && budget.ApprovedBudget > 0 ? "On Budget" : "Under Budget";
        return ProjectWorkspaceMapper.ToResponse(budget) with { RemainingBudget = budget.ApprovedBudget - budget.ActualCost, BudgetPercentageUsed = percentage, BudgetStatus = status };
    }

    private static RiskResponse ToResponse(ProjectRisk risk, Employee? owner) => ProjectWorkspaceMapper.ToResponse(risk) with { OwnerName = owner is null ? null : $"{owner.FirstName} {owner.LastName}" };
    private static TimelineEventResponse ToResponse(ProjectTimelineEvent item) => ProjectWorkspaceMapper.ToResponse(item);
    private static ActivityRecordResponse ToResponse(ProjectActivityRecord item) => ProjectWorkspaceMapper.ToResponse(item);
    private static ProjectResponse ToProjectResponse(Project project) => ProjectWorkspaceMapper.ToResponse(project) with { CustomerName = project.Customer?.Name, CategoryName = project.Category?.Name, BranchName = project.Branch?.Name, DepartmentName = project.Department?.Name, ProjectManagerName = project.ProjectManager is null ? null : $"{project.ProjectManager.FirstName} {project.ProjectManager.LastName}" };
    private static TeamMemberResponse ToResponse(ProjectTeamMember item, Employee employee) => ProjectWorkspaceMapper.ToResponse(item) with { EmployeeName = $"{employee.FirstName} {employee.LastName}" };
    private static AssetAllocationResponse ToResponse(ProjectAssetAllocation item, Asset asset) => ProjectWorkspaceMapper.ToResponse(item) with { AssetCode = asset.AssetCode, AssetName = asset.AssetName };
    private static DocumentResponse ToResponse(ProjectDocument item, Employee? employee) => ProjectWorkspaceMapper.ToResponse(item) with { UploadedByEmployeeName = employee is null ? null : $"{employee.FirstName} {employee.LastName}" };
}

internal static class ProjectWorkspaceExport
{
    public static byte[] BuildPdf(string text)
    {
        var lines = text.Split('\n').Select(x => x.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)").Replace("\r", string.Empty)).Take(45).ToList();
        var commands = new StringBuilder("BT\n/F1 10 Tf\n50 760 Td\n");
        foreach (var line in lines) commands.Append('(').Append(line.Length > 100 ? line[..100] : line).Append(") Tj\n0 -14 Td\n");
        commands.Append("ET");
        var stream = commands.ToString();
        var objects = new[]
        {
            "<< /Type /Catalog /Pages 2 0 R >>",
            "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
            "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>",
            "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>",
            $"<< /Length {Encoding.ASCII.GetByteCount(stream)} >>\nstream\n{stream}\nendstream"
        };
        var builder = new StringBuilder("%PDF-1.4\n");
        var offsets = new List<int> { 0 };
        for (var i = 0; i < objects.Length; i++) { offsets.Add(Encoding.ASCII.GetByteCount(builder.ToString())); builder.Append($"{i + 1} 0 obj\n{objects[i]}\nendobj\n"); }
        var xref = Encoding.ASCII.GetByteCount(builder.ToString());
        builder.Append($"xref\n0 {objects.Length + 1}\n0000000000 65535 f \n");
        foreach (var offset in offsets.Skip(1)) builder.Append($"{offset:D10} 00000 n \n");
        builder.Append($"trailer\n<< /Size {objects.Length + 1} /Root 1 0 R >>\nstartxref\n{xref}\n%%EOF");
        return Encoding.ASCII.GetBytes(builder.ToString());
    }

    public static byte[] BuildExcel(string text)
    {
        using var output = new MemoryStream();
        using (var archive = new ZipArchive(output, ZipArchiveMode.Create, leaveOpen: true))
        {
            Write(archive, "[Content_Types].xml", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\"><Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/><Default Extension=\"xml\" ContentType=\"application/xml\"/><Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/><Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/></Types>");
            Write(archive, "_rels/.rels", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\"><Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/></Relationships>");
            Write(archive, "xl/workbook.xml", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\"><sheets><sheet name=\"Report\" sheetId=\"1\" r:id=\"rId1\"/></sheets></workbook>");
            Write(archive, "xl/_rels/workbook.xml.rels", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\"><Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/></Relationships>");
            var rows = string.Join(string.Empty, text.Split('\n').Select((line, index) => $"<row r=\"{index + 1}\"><c r=\"A{index + 1}\" t=\"inlineStr\"><is><t>{SecurityElement.Escape(line.Trim())}</t></is></c></row>"));
            Write(archive, "xl/worksheets/sheet1.xml", $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><sheetData>{rows}</sheetData></worksheet>");
        }
        return output.ToArray();
    }

    private static void Write(ZipArchive archive, string name, string content)
    {
        var entry = archive.CreateEntry(name, CompressionLevel.Fastest);
        using var writer = new StreamWriter(entry.Open(), Encoding.UTF8);
        writer.Write(content);
    }
}
