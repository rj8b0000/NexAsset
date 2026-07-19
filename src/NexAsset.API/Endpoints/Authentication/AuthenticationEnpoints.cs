using System.Security.Claims;
using MediatR;
using NexAsset.API.Authorization;
using NexAsset.Infrastructure.Authorization;
using NexAsset.Application.Features.Authentication.Commands.Login;
using NexAsset.Application.Features.Authentication.Commands.Logout;
using NexAsset.Application.Features.Authentication.Commands.Register;
using NexAsset.Application.Features.Authentication.Commands.LockUser;
using NexAsset.Application.Features.Authentication.Commands.ResetPassword;
using NexAsset.Application.Features.Authentication.Commands.SetUserActive;
using NexAsset.Application.Features.Authentication.Commands.UnlockUser;
using NexAsset.Application.Features.Authentication.Queries.GetCurrentUser;

namespace NexAsset.API.Endpoints.Authentication;

public static class AuthenticationEnpoints
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth");
        group.MapPost("/login",
            async (
                LoginCommand command,
                ISender sender
            ) =>
            {
                var result = await sender.Send(command);

                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }
                
                return Results.Ok(result.Value);
            });
        group.MapPost(
            "/register",
            async (
                RegisterCommand command,
                ISender sender) =>
            {
                var result = await sender.Send(command);

                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);
            })
            .RequireAuthorization()
            .AddEndpointFilter<PermissionEnforcementFilter>();
        
        group.MapPost(
            "/logout",
            async (
                ISender sender) =>
            {
                var result = await sender.Send(
                    new LogoutCommand());

                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok();
            });
        
        group.MapGet(
                "/me",
                async (ISender sender) =>
                {
                    var result = await sender.Send(new GetCurrentUserQuery());

                    if (result.IsFailure)
                        return Results.Unauthorized();

                    return Results.Ok(result.Value);
            })
            .RequireAuthorization();

        // Effective permissions of the logged-in user (union of role and designation
        // permissions), used by the frontend to gate navigation and actions.
        group.MapGet(
                "/me/permissions",
                async (ClaimsPrincipal user, IEffectivePermissionService permissions, CancellationToken cancellationToken) =>
                {
                    var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (!Guid.TryParse(idClaim, out var userId))
                        return Results.Unauthorized();

                    var effective = await permissions.GetForUserAsync(userId, cancellationToken);
                    return Results.Ok(new
                    {
                        effective.IsSuperAdmin,
                        effective.OrganizationId,
                        effective.OrganizationName,
                        Permissions = effective.Permissions.OrderBy(p => p).ToList()
                    });
                })
            .RequireAuthorization();

        group.MapPost(
            "/reset-password",
            async (
                ResetPasswordCommand command,
                ISender sender) =>
            {
                var result = await sender.Send(command);

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .AddEndpointFilter<PermissionEnforcementFilter>();

        group.MapPost(
            "/set-active",
            async (
                SetUserActiveCommand command,
                ISender sender) =>
            {
                var result = await sender.Send(command);

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .AddEndpointFilter<PermissionEnforcementFilter>();

        group.MapPost(
            "/lock",
            async (
                LockUserCommand command,
                ISender sender) =>
            {
                var result = await sender.Send(command);

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .AddEndpointFilter<PermissionEnforcementFilter>();

        group.MapPost(
            "/unlock",
            async (
                UnlockUserCommand command,
                ISender sender) =>
            {
                var result = await sender.Send(command);

                if (result.IsFailure)
                    return Results.BadRequest(result.Error);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .AddEndpointFilter<PermissionEnforcementFilter>();
        return endpoints;
        
    }
}
