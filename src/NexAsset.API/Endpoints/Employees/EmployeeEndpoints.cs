using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.Application.Features.Employees.Commands.CreateEmployee;
using NexAsset.Application.Features.Employees.Commands.DeleteEmployee;
using NexAsset.Application.Features.Employees.Commands.UpdateEmployee;
using NexAsset.Application.Features.Employees.Queries.GetEmployee;
using NexAsset.Application.Features.Employees.Queries.GetEmployees;

namespace NexAsset.API.Endpoints.Employees;

public static class EmployeeEndpoints
{
    public static IEndpointRouteBuilder MapEmployeeEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/employees")
            .WithTags("Employees");

        group.MapPost("/", async (
            [FromBody] CreateEmployeeCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Created($"/api/employees/{result.Value!.Id}", result.Value);
        });

        group.MapGet("/", async (
            [AsParameters] GetEmployeesQuery query,
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
            var result = await sender.Send(new GetEmployeeQuery(id));
            if (result.IsFailure)
                return Results.NotFound(result.Error);

            return Results.Ok(result.Value);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            [FromBody] UpdateEmployeeRequest body,
            ISender sender) =>
        {
            var command = new UpdateEmployeeCommand(
                id,
                body.EmployeeCode,
                body.FirstName,
                body.LastName,
                body.Email,
                body.Phone,
                body.OrganizationId,
                body.BranchId,
                body.DepartmentId,
                body.DesignationId,
                body.ReportingManagerId,
                body.JoiningDate,
                body.EmploymentStatus,
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
            var result = await sender.Send(new DeleteEmployeeCommand(id));
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.NoContent();
        });

        return app;
    }
}
