using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.Application.Features.ProjectWorkspaces;

namespace NexAsset.API.Endpoints.ProjectWorkspaces;

public static class ProjectWorkspaceEndpoints
{
    public static IEndpointRouteBuilder MapProjectWorkspaceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/project-workspace")
            .WithTags("Project Workspace");

        MapCategories(group);
        MapProjects(group);
        MapDrafts(group);
        MapTeam(group);
        MapAssets(group);
        MapParameters(group);
        MapDocuments(group);
        MapActivities(group);
        MapOperationsHub(group);

        return app;
    }

    private static void MapCategories(RouteGroupBuilder group)
    {
        var categories = group.MapGroup("/categories");
        categories.MapPost("/", async ([FromBody] CreateProjectCategoryCommand command, ISender sender) => ToCreated(await sender.Send(command), "api/project-workspace/categories"));
        categories.MapGet("/", async ([AsParameters] GetProjectCategoriesQuery query, ISender sender) => ToResult(await sender.Send(query)));
        categories.MapGet("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new GetProjectCategoryQuery(id))));
        categories.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateProjectCategoryCommand body, ISender sender) => ToResult(await sender.Send(body with { Id = id })));
        categories.MapDelete("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new DeleteProjectCategoryCommand(id))));
    }

    private static void MapProjects(RouteGroupBuilder group)
    {
        var projects = group.MapGroup("/projects");
        projects.MapPost("/", async ([FromBody] CreateProjectCommand command, ISender sender) => ToCreated(await sender.Send(command), "api/project-workspace/projects"));
        projects.MapGet("/", async ([AsParameters] GetProjectsQuery query, ISender sender) => ToResult(await sender.Send(query)));
        projects.MapGet("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new GetProjectQuery(id))));
        projects.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateProjectCommand body, ISender sender) => ToResult(await sender.Send(body with { Id = id })));
        projects.MapDelete("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new DeleteProjectCommand(id))));
        projects.MapPost("/{id:guid}/archive", async (Guid id, ISender sender) => ToResult(await sender.Send(new ArchiveProjectCommand(id))));
        projects.MapPost("/{id:guid}/duplicate", async (Guid id, [FromBody] DuplicateProjectCommand body, ISender sender) => ToCreated(await sender.Send(body with { Id = id }), "api/project-workspace/projects"));
    }

    private static void MapDrafts(RouteGroupBuilder group)
    {
        var drafts = group.MapGroup("/drafts");
        drafts.MapPost("/", async ([FromBody] UpsertProjectDraftCommand command, ISender sender) => ToResult(await sender.Send(command)));
        drafts.MapGet("/", async ([AsParameters] GetProjectDraftsQuery query, ISender sender) => ToResult(await sender.Send(query)));
        drafts.MapGet("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new GetProjectDraftQuery(id))));
    }

    private static void MapTeam(RouteGroupBuilder group)
    {
        var members = group.MapGroup("/projects/{projectId:guid}/members");
        members.MapGet("/", async (Guid projectId, ISender sender) => ToResult(await sender.Send(new GetProjectMembersQuery(projectId))));
        members.MapPost("/", async (Guid projectId, [FromBody] AddProjectMemberCommand body, ISender sender) => ToResult(await sender.Send(body with { ProjectId = projectId })));

        group.MapPut("/members/{id:guid}", async (Guid id, [FromBody] UpdateProjectMemberCommand body, ISender sender) => ToResult(await sender.Send(body with { Id = id })));
        group.MapDelete("/members/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new RemoveProjectMemberCommand(id))));
    }

    private static void MapAssets(RouteGroupBuilder group)
    {
        var allocations = group.MapGroup("/projects/{projectId:guid}/asset-allocations");
        allocations.MapGet("/", async (Guid projectId, ISender sender) => ToResult(await sender.Send(new GetProjectAssetAllocationsQuery(projectId))));
        allocations.MapPost("/", async (Guid projectId, [FromBody] AllocateProjectAssetCommand body, ISender sender) => ToResult(await sender.Send(body with { ProjectId = projectId })));

        group.MapPost("/asset-allocations/{id:guid}/return", async (Guid id, [FromBody] ReturnProjectAssetCommand body, ISender sender) => ToResult(await sender.Send(body with { Id = id })));
        group.MapGet("/assets/{assetId:guid}/history", async (Guid assetId, ISender sender) => ToResult(await sender.Send(new GetAssetProjectHistoryQuery(assetId))));
    }

    private static void MapParameters(RouteGroupBuilder group)
    {
        var parameterGroups = group.MapGroup("/projects/{projectId:guid}/parameter-groups");
        parameterGroups.MapGet("/", async (Guid projectId, ISender sender) => ToResult(await sender.Send(new GetProjectParameterGroupsQuery(projectId))));
        parameterGroups.MapPost("/", async (Guid projectId, [FromBody] CreateProjectParameterGroupCommand body, ISender sender) => ToResult(await sender.Send(body with { ProjectId = projectId })));

        group.MapPut("/parameter-groups/{id:guid}", async (Guid id, [FromBody] UpdateProjectParameterGroupCommand body, ISender sender) => ToResult(await sender.Send(body with { Id = id })));
        group.MapDelete("/parameter-groups/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new DeleteProjectParameterGroupCommand(id))));

        var parameters = group.MapGroup("/projects/{projectId:guid}/parameters");
        parameters.MapGet("/", async (Guid projectId, ISender sender) => ToResult(await sender.Send(new GetProjectParametersQuery(projectId))));
        parameters.MapPost("/", async (Guid projectId, [FromBody] CreateProjectParameterCommand body, ISender sender) => ToResult(await sender.Send(body with { ProjectId = projectId })));

        group.MapPut("/parameters/{id:guid}", async (Guid id, [FromBody] UpdateProjectParameterCommand body, ISender sender) => ToResult(await sender.Send(body with { Id = id })));
        group.MapDelete("/parameters/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new DeleteProjectParameterCommand(id))));
    }

    private static void MapDocuments(RouteGroupBuilder group)
    {
        var documents = group.MapGroup("/projects/{projectId:guid}/documents");
        documents.MapGet("/", async (Guid projectId, ISender sender) => ToResult(await sender.Send(new GetProjectDocumentsQuery(projectId))));
        documents.MapPost("/", async (Guid projectId, [FromBody] AddProjectDocumentCommand body, ISender sender) => ToResult(await sender.Send(body with { ProjectId = projectId })));

        group.MapPut("/documents/{id:guid}", async (Guid id, [FromBody] ReplaceProjectDocumentCommand body, ISender sender) => ToResult(await sender.Send(body with { Id = id })));
        group.MapDelete("/documents/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new DeleteProjectDocumentCommand(id))));
    }

    private static void MapActivities(RouteGroupBuilder group)
    {
        group.MapGet("/projects/{projectId:guid}/activities", async (Guid projectId, ISender sender) => ToResult(await sender.Send(new GetProjectActivitiesQuery(projectId))));
    }

    private static void MapOperationsHub(RouteGroupBuilder group)
    {
        var projectGroup = group.MapGroup("/projects/{projectId:guid}");
        
        projectGroup.MapGet("/dashboard-kpis", async (Guid projectId, ISender sender) => ToResult(await sender.Send(new GetProjectDashboardKpisQuery(projectId))));
        
        projectGroup.MapGet("/budget", async (Guid projectId, ISender sender) => ToResult(await sender.Send(new GetProjectBudgetQuery(projectId))));
        projectGroup.MapPost("/budget", async (Guid projectId, [FromBody] UpsertProjectBudgetCommand body, ISender sender) => ToResult(await sender.Send(body with { ProjectId = projectId })));
        
        projectGroup.MapGet("/risks", async (Guid projectId, ISender sender) => ToResult(await sender.Send(new GetProjectRisksQuery(projectId))));
        projectGroup.MapPost("/risks", async (Guid projectId, [FromBody] CreateProjectRiskCommand body, ISender sender) => ToResult(await sender.Send(body with { ProjectId = projectId })));
        group.MapPut("/risks/{id:guid}", async (Guid id, [FromBody] UpdateProjectRiskCommand body, ISender sender) => ToResult(await sender.Send(body with { Id = id })));
        group.MapDelete("/risks/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new DeleteProjectRiskCommand(id))));
        
        projectGroup.MapGet("/settings", async (Guid projectId, ISender sender) => ToResult(await sender.Send(new GetProjectSettingsQuery(projectId))));
        projectGroup.MapPost("/settings", async (Guid projectId, [FromBody] UpsertProjectSettingCommand body, ISender sender) => ToResult(await sender.Send(body with { ProjectId = projectId })));
    }

    private static IResult ToCreated<T>(NexAsset.Application.Common.Results.Result<T> result, string basePath)
    {
        if (result.IsFailure) return Results.BadRequest(result.Error);
        var id = result.Value?.GetType().GetProperty("Id")?.GetValue(result.Value);
        return Results.Created($"/{basePath}/{id}", result.Value);
    }

    private static IResult ToResult<T>(NexAsset.Application.Common.Results.Result<T> result) =>
        result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);

    private static IResult ToResult(NexAsset.Application.Common.Results.Result result) =>
        result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
}
