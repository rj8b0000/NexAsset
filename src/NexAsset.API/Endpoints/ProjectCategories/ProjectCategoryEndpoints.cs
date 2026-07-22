using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.API.Authorization;
using NexAsset.Application.Features.ProjectCategories;

namespace NexAsset.API.Endpoints.ProjectCategories;

public static class ProjectCategoryEndpoints
{
    public static IEndpointRouteBuilder MapProjectCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/project-categories").WithTags("Project Categories").RequireAuthorization().AddEndpointFilter<PermissionEnforcementFilter>();
        group.MapGet("/", async ([AsParameters] GetProjectCategoriesQuery query, ISender sender) =>
        {
            var result = await sender.Send(query);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });
        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProjectCategoryQuery(id));
            return result.IsFailure ? Results.NotFound(result.Error) : Results.Ok(result.Value);
        });
        group.MapPost("/", async ([FromBody] CreateProjectCategoryCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Created($"/api/project-categories/{result.Value!.Id}", result.Value);
        });
        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateProjectCategoryCommand command, ISender sender) =>
        {
            var result = await sender.Send(command with { Id = id });
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });
        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProjectCategoryCommand(id));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
        });
        return app;
    }
}
