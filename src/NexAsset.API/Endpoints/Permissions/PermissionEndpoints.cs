using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.Application.Features.Permissions.Commands.AssignPermissionToRole;
using NexAsset.Application.Features.Permissions.Commands.CreatePermission;
using NexAsset.Application.Features.Permissions.Commands.DeletePermission;
using NexAsset.Application.Features.Permissions.Commands.RemovePermissionFromRole;
using NexAsset.Application.Features.Permissions.Commands.UpdatePermission;
using NexAsset.Application.Features.Permissions.Queries.GetPermission;
using NexAsset.Application.Features.Permissions.Queries.GetPermissions;
using NexAsset.Application.Features.Permissions.Queries.GetRolePermissions;

namespace NexAsset.API.Endpoints.Permissions;

public static class PermissionEndpoints
{
    public static IEndpointRouteBuilder MapPermissionEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/permissions")
            .WithTags("Permissions");

        group.MapPost("/", async (
            [FromBody] CreatePermissionCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Created($"/api/permissions/{result.Value!.Id}", result.Value);
        });

        group.MapGet("/", async (
            [AsParameters] GetPermissionsQuery query,
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
            var result = await sender.Send(new GetPermissionQuery(id));
            if (result.IsFailure)
                return Results.NotFound(result.Error);

            return Results.Ok(result.Value);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            [FromBody] UpdatePermissionRequest body,
            ISender sender) =>
        {
            var command = new UpdatePermissionCommand(
                id,
                body.Code,
                body.Name,
                body.Description,
                body.IsActive);

            var result = await sender.Send(command);
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Value);
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(new DeletePermissionCommand(id));
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.NoContent();
        });

        group.MapPost("/roles/assign", async (
            [FromBody] AssignPermissionToRoleCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.NoContent();
        });

        group.MapDelete("/roles/{roleId:guid}/{permissionId:guid}", async (
            Guid roleId,
            Guid permissionId,
            ISender sender) =>
        {
            var result = await sender.Send(
                new RemovePermissionFromRoleCommand(roleId, permissionId));
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.NoContent();
        });

        group.MapGet("/roles/{roleId:guid}", async (
            Guid roleId,
            ISender sender) =>
        {
            var result = await sender.Send(new GetRolePermissionsQuery(roleId));
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Value);
        });

        return app;
    }
}
