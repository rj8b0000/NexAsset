using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.Application.Features.Roles.Commands.AssignRole;
using NexAsset.Application.Features.Roles.Commands.CreateRole;
using NexAsset.Application.Features.Roles.Commands.DeleteRole;
using NexAsset.Application.Features.Roles.Commands.UpdateRole;
using NexAsset.Application.Features.Roles.Queries.GetRole;
using NexAsset.Application.Features.Roles.Queries.GetRoles;

namespace NexAsset.API.Endpoints.Roles;

public static class RoleEndpoints
{
    public static IEndpointRouteBuilder MapRoleEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/roles")
            .WithTags("Roles");

        group.MapPost("/", async (
            [FromBody] CreateRoleCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Created($"/api/roles/{result.Value!.Id}", result.Value);
        });

        group.MapGet("/", async (
            [AsParameters] GetRolesQuery query,
            ISender sender) =>
        {
            var result = await sender.Send(query);
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Value);
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(new GetRoleQuery(id));
            if (result.IsFailure)
                return Results.NotFound(result.Error);

            return Results.Ok(result.Value);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            [FromBody] UpdateRoleRequest body,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateRoleCommand(id, body.Name));
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Value);
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(new DeleteRoleCommand(id));
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.NoContent();
        });

        group.MapPost("/assign", async (
            [FromBody] AssignRoleCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.NoContent();
        });

        return app;
    }
}
