using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.Application.Features.Departments.Commands.CreateDepartment;
using NexAsset.Application.Features.Departments.Commands.DeleteDepartment;
using NexAsset.Application.Features.Departments.Commands.UpdateDepartment;
using NexAsset.Application.Features.Departments.Queries.GetDepartment;
using NexAsset.Application.Features.Departments.Queries.GetDepartments;

namespace NexAsset.API.Endpoints.Departments;

public static class DepartmentEndpoints
{
    public static IEndpointRouteBuilder MapDepartmentEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/departments")
            .WithTags("Departments");

        group.MapPost(
            "/",
            async (
                [FromBody] CreateDepartmentCommand command,
                ISender sender) =>
            {
                var result = await sender.Send(command);

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.Created(
                    $"/api/departments/{result.Value!.Id}",
                    result.Value);
            });

        group.MapGet(
            "/",
            async (
                [AsParameters] GetDepartmentsQuery query,
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
                var result = await sender.Send(new GetDepartmentQuery(id));

                if (result.IsFailure)
                    return Results.NotFound(result.Error);

                return Results.Ok(result.Value);
            });

        group.MapPut(
            "/{id:guid}",
            async (
                Guid id,
                [FromBody] UpdateDepartmentRequest body,
                ISender sender) =>
            {
                var command = new UpdateDepartmentCommand(
                    id,
                    body.OrganizationId,
                    body.Code,
                    body.Name,
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
                var result = await sender.Send(new DeleteDepartmentCommand(id));

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.NoContent();
            });

        return app;
    }
}
