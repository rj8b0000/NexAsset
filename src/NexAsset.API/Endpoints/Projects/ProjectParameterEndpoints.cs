using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.API.Authorization;
using NexAsset.Application.Features.ProjectParameters.Commands.AddParameter;
using NexAsset.Application.Features.ProjectParameters.Commands.CreateParameterSection;
using NexAsset.Application.Features.ProjectParameters.Commands.DeleteParameter;
using NexAsset.Application.Features.ProjectParameters.Commands.DeleteParameterSection;
using NexAsset.Application.Features.ProjectParameters.Commands.SaveParameterValues;
using NexAsset.Application.Features.ProjectParameters.Commands.UpdateParameter;
using NexAsset.Application.Features.ProjectParameters.Commands.UpdateParameterSection;
using NexAsset.Application.Features.ProjectParameters.Queries.GetParameterSections;

namespace NexAsset.API.Endpoints.Projects;

public static class ProjectParameterEndpoints
{
    public static IEndpointRouteBuilder MapProjectParameterEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects/{projectId:guid}/parameters")
            .WithTags("Project Parameters")
            .RequireAuthorization()
            .AddEndpointFilter<PermissionEnforcementFilter>();

        group.MapGet("/", async (Guid projectId, ISender sender) =>
        {
            var result = await sender.Send(new GetParameterSectionsQuery(projectId));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapPost("/sections", async (Guid projectId, [FromBody] CreateParameterSectionCommand body, ISender sender) =>
        {
            var command = body with { ProjectId = projectId };
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Created($"/api/projects/{projectId}/parameters/sections/{result.Value!.Id}", result.Value);
        });

        group.MapPut("/sections/{sectionId:guid}", async (Guid projectId, Guid sectionId, [FromBody] UpdateParameterSectionCommand body, ISender sender) =>
        {
            var command = body with { Id = sectionId };
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok();
        });

        group.MapDelete("/sections/{sectionId:guid}", async (Guid projectId, Guid sectionId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteParameterSectionCommand(sectionId));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
        });

        group.MapPost("/sections/{sectionId:guid}/fields", async (Guid projectId, Guid sectionId, [FromBody] AddParameterCommand body, ISender sender) =>
        {
            var command = body with { SectionId = sectionId };
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Created($"/api/projects/{projectId}/parameters/fields/{result.Value!.Id}", result.Value);
        });

        group.MapPut("/fields/{fieldId:guid}", async (Guid projectId, Guid fieldId, [FromBody] UpdateParameterCommand body, ISender sender) =>
        {
            var command = body with { Id = fieldId };
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapDelete("/fields/{fieldId:guid}", async (Guid projectId, Guid fieldId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteParameterCommand(fieldId));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
        });

        group.MapPost("/values", async (Guid projectId, [FromBody] SaveParameterValuesCommand body, ISender sender) =>
        {
            var command = body with { ProjectId = projectId };
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok();
        });

        return app;
    }
}
