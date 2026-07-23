using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.API.Authorization;
using NexAsset.Application.Features.Projects.Commands.CreateProject;
using NexAsset.Application.Features.Projects.Commands.DeleteDraftSession;
using NexAsset.Application.Features.Projects.Commands.DeleteProject;
using NexAsset.Application.Features.Projects.Commands.DuplicateProject;
using NexAsset.Application.Features.Projects.Commands.TransitionProjectStatus;
using NexAsset.Application.Features.Projects.Commands.UpdateProject;
using NexAsset.Application.Features.Projects.Commands.UpsertDraftSession;
using NexAsset.Application.Features.Projects.Queries.GetDraftSession;
using NexAsset.Application.Features.Projects.Queries.GetProject;
using NexAsset.Application.Features.Projects.Queries.GetProjects;

namespace NexAsset.API.Endpoints.Projects;

public static class ProjectEndpoints
{
    public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects")
            .WithTags("Projects")
            .RequireAuthorization()
            .AddEndpointFilter<PermissionEnforcementFilter>();

        // Projects Core CRUD
        group.MapPost("/", async ([FromBody] CreateProjectCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Created($"/api/projects/{result.Value!.Id}", result.Value);
        });

        group.MapGet("/", async ([AsParameters] GetProjectsQuery query, ISender sender) =>
        {
            var result = await sender.Send(query);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProjectQuery(id));
            return result.IsFailure ? Results.NotFound(result.Error) : Results.Ok(result.Value);
        });

        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateProjectCommand body, ISender sender) =>
        {
            var command = body with { Id = id };
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProjectCommand(id));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
        });

        group.MapPost("/{id:guid}/transition-status", async (Guid id, [FromBody] TransitionProjectStatusCommand body, ISender sender) =>
        {
            var command = body with { Id = id };
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapPost("/{id:guid}/duplicate", async (Guid id, [FromQuery] Guid organizationId, ISender sender) =>
        {
            var result = await sender.Send(new DuplicateProjectCommand(id, organizationId));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Created($"/api/projects/{result.Value!.Id}", result.Value);
        });

        // Draft Sessions (Autosave)
        group.MapPost("/draft-sessions", async ([FromBody] UpsertDraftSessionCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapGet("/draft-sessions", async ([FromQuery] Guid userId, [FromQuery] Guid organizationId, ISender sender) =>
        {
            var result = await sender.Send(new GetDraftSessionQuery(userId, organizationId));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapDelete("/draft-sessions", async ([FromQuery] Guid userId, [FromQuery] Guid organizationId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteDraftSessionCommand(userId, organizationId));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
        });

        return app;
    }
}
