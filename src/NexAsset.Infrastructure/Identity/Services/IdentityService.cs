using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Authentication.Commands.Login;
using NexAsset.Application.Features.Authentication.Commands.Register;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Identity.Services;

public sealed class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public IdentityService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<LoginResponse>.Failure("Invalid email or password");
        }
        
        if(!user.IsActive)
        {
            return Result<LoginResponse>.Failure("User account is not active");
        }
        
        var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false);
        if (!signInResult.Succeeded)
        {
            return Result<LoginResponse>.Failure("Invalid email or password");
        }
        
        user.LoginAtUtc = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return Result<LoginResponse>.Success(
            new LoginResponse(
                    user.Id,
                    user.Email!,
                    user.UserName ?? user.Email!
                )
        );
    }

    public async Task<Result> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return Result.Success();
    }

    public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
        {
            return Result<RegisterResponse>.Failure("Email already registered");
        }
        
        var organization = await _context.Organizations.FirstAsync(cancellationToken);

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true,
            OrganizationId = organization.Id,
            IsActive = true,
        };
        
        var result = await _userManager.CreateAsync(
            user,
            request.Password);
        
        if (!result.Succeeded)
        {
            return Result<RegisterResponse>.Failure(
                string.Join(", ", result.Errors.Select(x => x.Description)));
        }
        
        await _userManager.AddToRoleAsync(
            user,
            "Employee");

        return Result<RegisterResponse>.Success(
            new RegisterResponse(
                user.Id,
                user.Email!));
    }
    
}