using MediatR;
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
            });
        
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
            });

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
            });

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
            });

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
            });
        return endpoints;
        
    }
}
