using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.Application.Features.AssetReturns.Commands.ReturnAsset;
using NexAsset.Application.Features.AssetReturns.Queries.GetAssetReturnHistory;
using NexAsset.API.Authorization;

namespace NexAsset.API.Endpoints.AssetReturns;

public static class AssetReturnEndpoints
{
    public static IEndpointRouteBuilder MapAssetReturnEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/asset-returns").WithTags("Asset Returns")
            .RequireAuthorization()
            .AddEndpointFilter<PermissionEnforcementFilter>();

        group.MapPost("/", async ([FromBody] ReturnAssetCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapGet("/assets/{assetId:guid}/history", async (Guid assetId, ISender sender) =>
        {
            var result = await sender.Send(new GetAssetReturnHistoryQuery(assetId));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        return app;
    }
}
