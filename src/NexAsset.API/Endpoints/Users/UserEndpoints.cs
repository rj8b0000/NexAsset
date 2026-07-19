using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.API.Authorization;
using NexAsset.Application.Features.Authentication.Commands.LockUser;
using NexAsset.Application.Features.Authentication.Commands.ResetPassword;
using NexAsset.Application.Features.Authentication.Commands.SetUserActive;
using NexAsset.Application.Features.Authentication.Commands.UnlockUser;
using NexAsset.Application.Features.Roles.Commands.AssignRole;
using NexAsset.Application.Features.Users.Commands.CreateUser;
using NexAsset.Application.Features.Users.Commands.RemoveRole;
using NexAsset.Application.Features.Users.Queries.GetUsers;

namespace NexAsset.API.Endpoints.Users;

/// <summary>
/// Login-account administration: who can sign in, which roles they hold, and account state.
/// Distinct from Employees (HR records) — an employee may or may not have a login.
/// </summary>
public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users")
            .RequireAuthorization()
            .AddEndpointFilter<PermissionEnforcementFilter>();

        group.MapGet("/", async (
            [AsParameters] GetUsersQuery query,
            ISender sender) =>
        {
            var result = await sender.Send(query);
            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.Ok(result.Value);
        });

        group.MapPost("/", async (
            [FromBody] CreateUserCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.Ok(new { Id = result.Value });
        });

        group.MapPost("/{id:guid}/roles", async (
            Guid id,
            [FromBody] AssignRoleRequest body,
            ISender sender) =>
        {
            var result = await sender.Send(new AssignRoleCommand(id, body.RoleName));
            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.NoContent();
        });

        group.MapDelete("/{id:guid}/roles/{roleName}", async (
            Guid id,
            string roleName,
            ISender sender) =>
        {
            var result = await sender.Send(new RemoveRoleCommand(id, roleName));
            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.NoContent();
        });

        group.MapPost("/{id:guid}/active", async (
            Guid id,
            [FromBody] SetActiveRequest body,
            ISender sender) =>
        {
            var result = await sender.Send(new SetUserActiveCommand(id, body.IsActive));
            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.NoContent();
        });

        group.MapPost("/{id:guid}/lock", async (
            Guid id,
            [FromBody] LockRequest? body,
            ISender sender) =>
        {
            var result = await sender.Send(new LockUserCommand(id, body?.LockoutEnd));
            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.NoContent();
        });

        group.MapPost("/{id:guid}/unlock", async (
            Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(new UnlockUserCommand(id));
            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.NoContent();
        });

        group.MapPost("/{id:guid}/reset-password", async (
            Guid id,
            [FromBody] ResetPasswordRequest body,
            ISender sender) =>
        {
            var result = await sender.Send(new ResetPasswordCommand(id, body.NewPassword));
            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.NoContent();
        });

        return app;
    }

    private sealed record AssignRoleRequest(string RoleName);
    private sealed record SetActiveRequest(bool IsActive);
    private sealed record LockRequest(DateTimeOffset? LockoutEnd);
    private sealed record ResetPasswordRequest(string NewPassword);
}
