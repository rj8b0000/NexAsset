using System.Security.Claims;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Infrastructure.Authorization;

namespace NexAsset.Api.Middlewares;

/// <summary>
/// Establishes the organization boundary for the request before any handler runs, so the
/// DbContext's query filters know which organization the caller may read. Must be registered
/// after UseAuthentication and before the endpoints.
/// </summary>
public sealed class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITenantContext tenant,
        IEffectivePermissionService permissions)
    {
        var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (context.User.Identity?.IsAuthenticated == true && Guid.TryParse(idClaim, out var userId))
        {
            var effective = await permissions.GetForUserAsync(userId, context.RequestAborted);
            tenant.Apply(effective.OrganizationId, effective.IsSuperAdmin, isAuthenticated: true, ReadSelectedOrganization(context));
        }
        else
        {
            tenant.Apply(null, false, isAuthenticated: false, selectedOrganizationId: null);
        }

        await _next(context);
    }

    // The workspace switcher's selection, sent by the SPA. TenantContext ignores it for anyone
    // but a SuperAdmin, so a normal user can't widen their boundary by forging the header.
    private static Guid? ReadSelectedOrganization(HttpContext context)
        => context.Request.Headers.TryGetValue("X-Organization-Id", out var value)
           && Guid.TryParse(value, out var organizationId)
            ? organizationId
            : null;
}
