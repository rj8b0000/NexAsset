using System.Security.Claims;
using NexAsset.Infrastructure.Authorization;

namespace NexAsset.API.Authorization;

/// <summary>
/// Endpoint filter that enforces the permission derived by <see cref="PermissionRouteConvention"/>:
/// the caller must hold the required "Module.Action" permission through a role or their
/// designation. SuperAdmin bypasses all checks. Runs after authentication, so the group must also
/// have RequireAuthorization().
/// </summary>
public sealed class PermissionEnforcementFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var http = context.HttpContext;
        var required = PermissionRouteConvention.Resolve(http);
        if (required is null)
            return await next(context);

        var idClaim = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(idClaim, out var userId))
            return Results.Unauthorized();

        var permissionService = http.RequestServices.GetRequiredService<IEffectivePermissionService>();
        var effective = await permissionService.GetForUserAsync(userId, http.RequestAborted);
        if (effective.IsSuperAdmin || effective.Permissions.Contains(required))
            return await next(context);

        // Shape matches the frontend's { Message } error parser.
        return Results.Json(
            new { Message = $"You don't have permission to do this (requires '{required}')." },
            statusCode: StatusCodes.Status403Forbidden);
    }
}
