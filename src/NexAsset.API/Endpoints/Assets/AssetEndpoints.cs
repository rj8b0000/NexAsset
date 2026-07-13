using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.Application.Features.Assets.Commands.CreateAsset;
using NexAsset.Application.Features.Assets.Commands.DeleteAsset;
using NexAsset.Application.Features.Assets.Commands.UpdateAsset;
using NexAsset.Application.Features.Assets.Queries.GetAsset;
using NexAsset.Application.Features.Assets.Queries.GetAssets;

namespace NexAsset.API.Endpoints.Assets;

public static class AssetEndpoints
{
    public static IEndpointRouteBuilder MapAssetEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/assets").WithTags("Assets");

        group.MapPost("/", async ([FromBody] CreateAssetCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Created($"/api/assets/{result.Value!.Id}", result.Value);
        });

        group.MapGet("/", async ([AsParameters] GetAssetsQuery query, ISender sender) =>
        {
            var result = await sender.Send(query);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetAssetQuery(id));
            return result.IsFailure ? Results.NotFound(result.Error) : Results.Ok(result.Value);
        });

        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateAssetRequest body, ISender sender) =>
        {
            var command = new UpdateAssetCommand(id, body.OrganizationId, body.CategoryId, body.BranchId, body.DepartmentId, body.CurrentEmployeeId, body.AssetCode, body.AssetName, body.SerialNumber, body.Barcode, body.QrCode, body.Brand, body.Model, body.PurchaseDate, body.WarrantyExpiry, body.Vendor, body.PurchaseCost, body.CurrentValue, body.AssetStatus, body.Location, body.Remarks);
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteAssetCommand(id));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
        });

        return app;
    }
}
