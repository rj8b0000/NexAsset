using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.Application.Features.Organizations.Commands.CreateOrganization;
using NexAsset.Application.Features.Organizations.Commands.DeleteOrganization;
using NexAsset.Application.Features.Organizations.Commands.UpdateOrganization;
using NexAsset.Application.Features.Organizations.Queries.GetOrganization;
using NexAsset.Application.Features.Organizations.Queries.GetOrganizations;
using NexAsset.API.Authorization;

namespace NexAsset.API.Endpoints.Organizations;

public static class OrganizationEndpoints
{
    public static IEndpointRouteBuilder MapOrganizationEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/organizations")
            .WithTags("Organizations")
            .RequireAuthorization()
            .AddEndpointFilter<PermissionEnforcementFilter>();

        group.MapPost(
            "/",
            async (
                [FromBody] CreateOrganizationCommand command,
                ISender sender) =>
            {
                var result = await sender.Send(command);

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.Created(
                    $"/api/organizations/{result.Value!.Id}",
                    result.Value);
            });

        group.MapGet(
            "/",
            async (
                [AsParameters] GetOrganizationsQuery query,
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
                var result = await sender.Send(
                    new GetOrganizationQuery(id));

                if (result.IsFailure)
                    return Results.NotFound(result.Error);

                return Results.Ok(result.Value);
            });

        group.MapPut(
            "/{id:guid}",
            async (
                Guid id,
                [FromBody] UpdateOrganizationRequest body,
                ISender sender) =>
            {
                var command = new UpdateOrganizationCommand(
                    id,
                    body.Code,
                    body.Name,
                    body.LegalName,
                    body.Email,
                    body.Phone,
                    body.Website,
                    body.Address,
                    body.City,
                    body.State,
                    body.Country,
                    body.PostalCode,
                    body.Currency,
                    body.TimeZone,
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
                var result = await sender.Send(
                    new DeleteOrganizationCommand(id));

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.NoContent();
            });

        return app;
    }
}
