using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.Application.Features.AssetCategories.Commands.CreateAssetCategory;
using NexAsset.Application.Features.AssetCategories.Commands.DeleteAssetCategory;
using NexAsset.Application.Features.AssetCategories.Commands.UpdateAssetCategory;
using NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategories;
using NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategory;
using NexAsset.API.Authorization;

namespace NexAsset.API.Endpoints.AssetCategories;

public static class AssetCategoryEndpoints
{
    public static IEndpointRouteBuilder MapAssetCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/asset-categories").WithTags("Asset Categories")
            .RequireAuthorization()
            .AddEndpointFilter<PermissionEnforcementFilter>();

        group.MapPost("/", async ([FromBody] CreateAssetCategoryCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Created($"/api/asset-categories/{result.Value!.Id}", result.Value);
        });

        group.MapGet("/", async ([AsParameters] GetAssetCategoriesQuery query, ISender sender) =>
        {
            var result = await sender.Send(query);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetAssetCategoryQuery(id));
            return result.IsFailure ? Results.NotFound(result.Error) : Results.Ok(result.Value);
        });

        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateAssetCategoryRequest body, ISender sender) =>
        {
            var result = await sender.Send(new UpdateAssetCategoryCommand(id, body.OrganizationId, body.Code, body.Name, body.Description, body.IsActive));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteAssetCategoryCommand(id));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
        });

        return app;
    }
}
