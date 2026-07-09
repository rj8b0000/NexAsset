using Microsoft.AspNetCore.Identity;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Authentication.Commands.Login;
using NexAsset.Application.Features.Authentication.Commands.Register;

namespace NexAsset.Infrastructure.Identity.Services;

public sealed class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result> LogoutAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
}