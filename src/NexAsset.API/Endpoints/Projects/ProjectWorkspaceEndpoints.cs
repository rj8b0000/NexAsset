using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.API.Authorization;
using NexAsset.Application.Features.Projects;
using NexAsset.Domain.Enums;

namespace NexAsset.API.Endpoints.Projects;

public static class ProjectWorkspaceEndpoints
{
    public static IEndpointRouteBuilder MapProjectWorkspaceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects").WithTags("Projects").RequireAuthorization().AddEndpointFilter<PermissionEnforcementFilter>();

        group.MapGet("/", async ([AsParameters] GetProjectsQuery query, ISender sender) => await SendList(sender, query));
        group.MapGet("/{id:guid}", async (Guid id, ISender sender) => await SendGet(sender, new GetProjectQuery(id)));
        group.MapPost("/", async ([FromBody] CreateProjectCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Created($"/api/projects/{result.Value!.Id}", result.Value);
        });
        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateProjectRequest body, ISender sender) => await SendMutation(sender, new UpdateProjectCommand(id, body.OrganizationId, body.CategoryId, body.CustomerId, body.BranchId, body.DepartmentId, body.ProjectManagerEmployeeId, body.ProjectCode, body.ProjectName, body.Description, body.Notes, body.InternalRemarks, body.Priority, body.StartDate, body.EndDate, body.ExpectedCompletionDate)));
        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) => await SendMutation(sender, new DeleteProjectCommand(id)));
        group.MapPost("/{id:guid}/status", async (Guid id, [FromBody] StatusRequest body, ISender sender) => await SendMutation(sender, new TransitionProjectStatusCommand(id, body.NewStatus)));
        group.MapPost("/{id:guid}/duplicate", async (Guid id, [FromBody] DuplicateRequest? body, ISender sender) =>
        {
            var result = await sender.Send(new DuplicateProjectCommand(id, body?.ProjectCode));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Created($"/api/projects/{result.Value!.Id}", result.Value);
        });

        group.MapGet("/{id:guid}/team", async (Guid id, TeamMemberStatus? status, int pageNumber, int pageSize, string? search, string? sortBy, bool descending, ISender sender) =>
            await SendList(sender, new GetTeamMembersQuery { ProjectId = id, Status = status, PageNumber = pageNumber, PageSize = pageSize, Search = search, SortBy = sortBy, Descending = descending }));
        group.MapPost("/{id:guid}/team", async (Guid id, [FromBody] AddTeamMemberCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id }));
        group.MapPut("/{id:guid}/team/{memberId:guid}", async (Guid id, Guid memberId, [FromBody] UpdateTeamMemberCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id, Id = memberId }));
        group.MapPost("/{id:guid}/team/{memberId:guid}/release", async (Guid id, Guid memberId, [FromBody] ReleaseTeamMemberCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id, Id = memberId }));
        group.MapDelete("/{id:guid}/team/{memberId:guid}", async (Guid id, Guid memberId, ISender sender) => await SendMutation(sender, new RemoveTeamMemberCommand(id, memberId)));

        group.MapGet("/{id:guid}/assets", async (Guid id, AllocationStatus? status, int pageNumber, int pageSize, string? search, ISender sender) =>
            await SendList(sender, new GetAssetAllocationsQuery { ProjectId = id, Status = status, PageNumber = pageNumber, PageSize = pageSize, Search = search }));
        group.MapPost("/{id:guid}/assets", async (Guid id, [FromBody] AllocateAssetCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id }));
        group.MapPost("/{id:guid}/assets/{allocationId:guid}/return", async (Guid id, Guid allocationId, [FromBody] ReturnAssetCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id, Id = allocationId }));

        group.MapGet("/{id:guid}/documents", async (Guid id, DocumentCategory? category, int pageNumber, int pageSize, string? search, string? sortBy, bool descending, ISender sender) =>
            await SendList(sender, new GetDocumentsQuery { ProjectId = id, Category = category, PageNumber = pageNumber, PageSize = pageSize, Search = search, SortBy = sortBy, Descending = descending }));
        group.MapPost("/{id:guid}/documents", async (Guid id, [FromBody] UploadDocumentCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id }));
        group.MapPost("/{id:guid}/documents/{documentId:guid}/replace", async (Guid id, Guid documentId, [FromBody] ReplaceDocumentCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id, Id = documentId }));
        group.MapDelete("/{id:guid}/documents/{documentId:guid}", async (Guid id, Guid documentId, ISender sender) => await SendMutation(sender, new DeleteDocumentCommand(id, documentId)));
        group.MapGet("/{id:guid}/documents/{documentName}/versions", async (Guid id, string documentName, ISender sender) => await SendList(sender, new GetDocumentVersionHistoryQuery(id, documentName)));

        group.MapGet("/{id:guid}/parameters", async (Guid id, ISender sender) => await SendList(sender, new GetParameterSectionsQuery(id)));
        group.MapPost("/{id:guid}/parameters/sections", async (Guid id, [FromBody] CreateParameterSectionCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id }));
        group.MapPut("/{id:guid}/parameters/sections/{sectionId:guid}", async (Guid id, Guid sectionId, [FromBody] UpdateParameterSectionCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id, Id = sectionId }));
        group.MapDelete("/{id:guid}/parameters/sections/{sectionId:guid}", async (Guid id, Guid sectionId, ISender sender) => await SendMutation(sender, new DeleteParameterSectionCommand(id, sectionId)));
        group.MapPost("/{id:guid}/parameters/sections/{sectionId:guid}/parameters", async (Guid id, Guid sectionId, [FromBody] AddParameterCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id, SectionId = sectionId }));
        group.MapPut("/{id:guid}/parameters/sections/{sectionId:guid}/parameters/{parameterId:guid}", async (Guid id, Guid sectionId, Guid parameterId, [FromBody] UpdateParameterCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id, SectionId = sectionId, Id = parameterId }));
        group.MapDelete("/{id:guid}/parameters/sections/{sectionId:guid}/parameters/{parameterId:guid}", async (Guid id, Guid sectionId, Guid parameterId, ISender sender) => await SendMutation(sender, new DeleteParameterCommand(id, sectionId, parameterId)));
        group.MapPut("/{id:guid}/parameters/values", async (Guid id, [FromBody] SaveParameterValuesCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id }));

        group.MapGet("/{id:guid}/budget", async (Guid id, ISender sender) => await SendGet(sender, new GetCurrentBudgetQuery(id)));
        group.MapPut("/{id:guid}/budget", async (Guid id, [FromBody] UpdateBudgetCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id }));
        group.MapGet("/{id:guid}/budget/history", async (Guid id, int pageNumber, int pageSize, ISender sender) => await SendList(sender, new GetBudgetHistoryQuery { ProjectId = id, PageNumber = pageNumber, PageSize = pageSize }));

        group.MapGet("/{id:guid}/risks", async (Guid id, RiskCategory? category, RiskProbability? probability, RiskImpact? impact, RiskSeverity? severity, RiskStatus? status, Guid? ownerEmployeeId, int pageNumber, int pageSize, string? search, string? sortBy, bool descending, ISender sender) =>
            await SendList(sender, new GetRisksQuery { ProjectId = id, Category = category, Probability = probability, Impact = impact, Severity = severity, Status = status, OwnerEmployeeId = ownerEmployeeId, PageNumber = pageNumber, PageSize = pageSize, Search = search, SortBy = sortBy, Descending = descending }));
        group.MapGet("/{id:guid}/risks/{riskId:guid}", async (Guid id, Guid riskId, ISender sender) => await SendGet(sender, new GetRiskQuery(id, riskId)));
        group.MapPost("/{id:guid}/risks", async (Guid id, [FromBody] CreateRiskCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id }));
        group.MapPut("/{id:guid}/risks/{riskId:guid}", async (Guid id, Guid riskId, [FromBody] UpdateRiskCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id, Id = riskId }));
        group.MapPost("/{id:guid}/risks/{riskId:guid}/close", async (Guid id, Guid riskId, [FromBody] CloseRiskCommand command, ISender sender) => await SendMutation(sender, command with { ProjectId = id, Id = riskId }));
        group.MapDelete("/{id:guid}/risks/{riskId:guid}", async (Guid id, Guid riskId, ISender sender) => await SendMutation(sender, new DeleteRiskCommand(id, riskId)));

        group.MapGet("/{id:guid}/timeline", async (Guid id, TimelineEventType? eventType, string? keyword, int pageNumber, int pageSize, ISender sender) => await SendList(sender, new GetTimelineQuery { ProjectId = id, EventType = eventType, Keyword = keyword, PageNumber = pageNumber, PageSize = pageSize }));
        group.MapGet("/{id:guid}/activities", async (Guid id, ActivityType? activityType, Guid? userId, DateTime? from, DateTime? to, string? keyword, int pageNumber, int pageSize, ISender sender) => await SendList(sender, new GetActivitiesQuery { ProjectId = id, ActivityType = activityType, UserId = userId, From = from, To = to, Keyword = keyword, PageNumber = pageNumber, PageSize = pageSize }));
        group.MapGet("/{id:guid}/dashboard", async (Guid id, ISender sender) => await SendGet(sender, new GetProjectDashboardQuery(id)));

        group.MapGet("/{id:guid}/reports/summary", async (Guid id, ISender sender) => await SendGet(sender, new GetProjectSummaryReportQuery(id)));
        group.MapGet("/{id:guid}/reports/team", async (Guid id, ISender sender) => await SendGet(sender, new GetTeamAllocationReportQuery(id)));
        group.MapGet("/{id:guid}/reports/assets", async (Guid id, ISender sender) => await SendGet(sender, new GetAssetAllocationReportQuery(id)));
        group.MapGet("/{id:guid}/reports/budget", async (Guid id, ISender sender) => await SendGet(sender, new GetBudgetReportQuery(id)));
        group.MapGet("/{id:guid}/reports/risks", async (Guid id, ISender sender) => await SendGet(sender, new GetRiskReportQuery(id)));
        group.MapGet("/{id:guid}/reports/documents", async (Guid id, ISender sender) => await SendGet(sender, new GetDocumentRegisterReportQuery(id)));
        group.MapGet("/{id:guid}/reports/activities", async (Guid id, ISender sender) => await SendGet(sender, new GetActivityReportQuery(id)));
        group.MapGet("/{id:guid}/reports/timeline", async (Guid id, ISender sender) => await SendGet(sender, new GetTimelineReportQuery(id)));
        group.MapGet("/{id:guid}/reports/parameters", async (Guid id, ISender sender) => await SendGet(sender, new GetParameterReportQuery(id)));
        group.MapPost("/{id:guid}/reports/export/pdf", async (Guid id, [FromBody] ExportRequest body, ISender sender) => await SendFile(sender, new ExportReportPdfCommand(id, body.ReportType)));
        group.MapPost("/{id:guid}/reports/export/excel", async (Guid id, [FromBody] ExportRequest body, ISender sender) => await SendFile(sender, new ExportReportExcelCommand(id, body.ReportType)));

        group.MapGet("/drafts", async (Guid organizationId, ISender sender) => await SendGet(sender, new GetDraftSessionQuery(organizationId)));
        group.MapPut("/drafts", async ([FromBody] UpsertDraftSessionCommand command, ISender sender) => await SendMutation(sender, command));
        group.MapDelete("/drafts", async (Guid organizationId, ISender sender) => await SendMutation(sender, new DeleteDraftSessionCommand(organizationId)));
        group.MapGet("/search", async ([AsParameters] GlobalSearchQuery query, ISender sender) => await SendList(sender, query));
        group.MapGet("/saved-filters", async (Guid organizationId, string? entityType, ISender sender) => await SendList(sender, new GetSavedFiltersQuery(organizationId, entityType)));
        group.MapPost("/saved-filters", async ([FromBody] SaveFilterCommand command, ISender sender) => await SendMutation(sender, command));
        group.MapDelete("/saved-filters/{filterId:guid}", async (Guid filterId, ISender sender) => await SendMutation(sender, new DeleteSavedFilterCommand(filterId)));
        return app;
    }

    private static async Task<IResult> SendMutation<T>(ISender sender, IRequest<NexAsset.Application.Common.Results.Result<T>> command)
    {
        var result = await sender.Send(command);
        return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
    }

    private static async Task<IResult> SendMutation(ISender sender, IRequest<NexAsset.Application.Common.Results.Result> command)
    {
        var result = await sender.Send(command);
        return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
    }

    private static async Task<IResult> SendGet<T>(ISender sender, IRequest<NexAsset.Application.Common.Results.Result<T>> query)
    {
        var result = await sender.Send(query);
        return result.IsFailure ? Results.NotFound(result.Error) : Results.Ok(result.Value);
    }

    private static async Task<IResult> SendList<T>(ISender sender, IRequest<NexAsset.Application.Common.Results.Result<T>> query)
    {
        var result = await sender.Send(query);
        return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
    }

    private static async Task<IResult> SendFile(ISender sender, IRequest<NexAsset.Application.Common.Results.Result<ReportExportResponse>> command)
    {
        var result = await sender.Send(command);
        return result.IsFailure ? Results.BadRequest(result.Error) : Results.File(result.Value!.Content, result.Value.ContentType, result.Value.FileName);
    }

    public sealed record StatusRequest(ProjectStatus NewStatus);
    public sealed record DuplicateRequest(string? ProjectCode);
    public sealed record ExportRequest(ProjectReportType ReportType);
}
