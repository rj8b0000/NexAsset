using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.Application.Features.AssetAssignments.Commands.AssignAsset;
using NexAsset.Application.Features.AssetAssignments.Commands.UnassignAsset;
using NexAsset.Application.Features.AssetAssignments.Queries.GetAssetAssignment;
using NexAsset.Application.Features.AssetAssignments.Queries.GetAssetAssignmentHistory;
using NexAsset.API.Authorization;

namespace NexAsset.API.Endpoints.AssetAssignments;

public static class AssetAssignmentEndpoints
{
    public static IEndpointRouteBuilder MapAssetAssignmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/asset-assignments").WithTags("Asset Assignments")
            .RequireAuthorization()
            .AddEndpointFilter<PermissionEnforcementFilter>();

        group.MapPost("/assign", async ([FromBody] AssignAssetCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapPost("/unassign", async ([FromBody] UnassignAssetCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
        });

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetAssetAssignmentQuery(id));
            return result.IsFailure ? Results.NotFound(result.Error) : Results.Ok(result.Value);
        });

        group.MapGet("/assets/{assetId:guid}/history", async (Guid assetId, ISender sender) =>
        {
            var result = await sender.Send(new GetAssetAssignmentHistoryQuery(assetId));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        return app;
    }
}
