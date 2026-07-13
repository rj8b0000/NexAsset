using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.Application.Features.Branches.Commands.CreateBranch;
using NexAsset.Application.Features.Branches.Commands.DeleteBranch;
using NexAsset.Application.Features.Branches.Commands.UpdateBranch;
using NexAsset.Application.Features.Branches.Queries.GetBranch;
using NexAsset.Application.Features.Branches.Queries.GetBranches;

namespace NexAsset.API.Endpoints.Branches;

public static class BranchEndpoints
{
    public static IEndpointRouteBuilder MapBranchEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/branches")
            .WithTags("Branches");

        group.MapPost(
            "/",
            async (
                [FromBody] CreateBranchCommand command,
                ISender sender) =>
            {
                var result = await sender.Send(command);

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.Created(
                    $"/api/branches/{result.Value!.Id}",
                    result.Value);
            });

        group.MapGet(
            "/",
            async (
                [AsParameters] GetBranchesQuery query,
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
                var result = await sender.Send(new GetBranchQuery(id));

                if (result.IsFailure)
                    return Results.NotFound(result.Error);

                return Results.Ok(result.Value);
            });

        group.MapPut(
            "/{id:guid}",
            async (
                Guid id,
                [FromBody] UpdateBranchRequest body,
                ISender sender) =>
            {
                var command = new UpdateBranchCommand(
                    id,
                    body.OrganizationId,
                    body.Code,
                    body.Name,
                    body.Email,
                    body.Phone,
                    body.Address,
                    body.City,
                    body.State,
                    body.Country,
                    body.PostalCode,
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
                var result = await sender.Send(new DeleteBranchCommand(id));

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.NoContent();
            });

        return app;
    }
}
