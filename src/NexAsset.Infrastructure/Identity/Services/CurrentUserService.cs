using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NexAsset.Application.Common.Interfaces;

namespace NexAsset.Infrastructure.Identity.Services;

public class CurrentUserService: ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var id = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(id, out var guid)
                ? guid
                : null;
        }
    }
    
    public string? Email =>
        User?.FindFirstValue(ClaimTypes.Email);

    public string? UserName =>
        User?.Identity?.Name;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;
}