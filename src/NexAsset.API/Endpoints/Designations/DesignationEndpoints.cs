using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.Application.Features.Designations.Commands.CreateDesignation;
using NexAsset.Application.Features.Designations.Commands.DeleteDesignation;
using NexAsset.Application.Features.Designations.Commands.UpdateDesignation;
using NexAsset.Application.Features.Designations.Queries.GetDesignation;
using NexAsset.Application.Features.Designations.Queries.GetDesignations;

namespace NexAsset.API.Endpoints.Designations;

public static class DesignationEndpoints
{
    public static IEndpointRouteBuilder MapDesignationEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/designations")
            .WithTags("Designations");

        group.MapPost(
            "/",
            async (
                [FromBody] CreateDesignationCommand command,
                ISender sender) =>
            {
                var result = await sender.Send(command);

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.Created(
                    $"/api/designations/{result.Value!.Id}",
                    result.Value);
            });

        group.MapGet(
            "/",
            async (
                [AsParameters] GetDesignationsQuery query,
                ISender sender) =>
            {
                var result = await sender.Send(query);

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.Ok(result.Value);
            });

        group.MapGet(
            "/{id:guid}",
            async (
                Guid id,
                ISender sender) =>
            {
                var result = await sender.Send(new GetDesignationQuery(id));

                if (result.IsFailure)
                    return Results.NotFound(result.Error);

                return Results.Ok(result.Value);
            });

        group.MapPut(
            "/{id:guid}",
            async (
                Guid id,
                [FromBody] UpdateDesignationRequest body,
                ISender sender) =>
            {
                var command = new UpdateDesignationCommand(
                    id,
                    body.OrganizationId,
                    body.DepartmentId,
                    body.Title,
                    body.Description,
                    body.IsActive);

                var result = await sender.Send(command);

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.Ok(result.Value);
            });

        group.MapDelete(
            "/{id:guid}",
            async (
                Guid id,
                ISender sender) =>
            {
                var result = await sender.Send(new DeleteDesignationCommand(id));

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.NoContent();
            });

        return app;
    }
}
