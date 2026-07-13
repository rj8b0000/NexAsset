using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Identity;
using NexAsset.Application.Common.Models.Paging;
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

    public async Task<Result<Guid>> CreateEmployeeUserAsync(
        CreateEmployeeUserRequest request,
        CancellationToken cancellationToken)
    {
        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
            return Result<Guid>.Failure("Email already registered.");

        foreach (var role in request.Roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                return Result<Guid>.Failure($"Role '{role}' not found.");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true,
            OrganizationId = request.OrganizationId,
            BranchId = request.BranchId,
            DepartmentId = request.DepartmentId,
            DesignationId = request.DesignationId,
            EmployeeId = request.EmployeeId,
            IsActive = true
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
            return Result<Guid>.Failure(FormatErrors(createResult));

        if (request.Roles.Count > 0)
        {
            var roleResult = await _userManager.AddToRolesAsync(user, request.Roles);

            if (!roleResult.Succeeded)
                return Result<Guid>.Failure(FormatErrors(roleResult));
        }

        return Result<Guid>.Success(user.Id);
    }

    public async Task<Result> UpdateEmployeeUserAsync(
        Guid userId,
        Guid organizationId,
        Guid? branchId,
        Guid? departmentId,
        Guid? designationId,
        string email,
        bool isActive,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.Failure("User not found.");

        var existingEmailUser = await _userManager.FindByEmailAsync(email);
        if (existingEmailUser is not null && existingEmailUser.Id != userId)
            return Result.Failure("Email already registered.");

        user.UserName = email;
        user.Email = email;
        user.OrganizationId = organizationId;
        user.BranchId = branchId;
        user.DepartmentId = departmentId;
        user.DesignationId = designationId;
        user.IsActive = isActive;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(FormatErrors(result));
    }

    public async Task<Result> SetUserActiveAsync(
        Guid userId,
        bool isActive,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.Failure("User not found.");

        user.IsActive = isActive;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(FormatErrors(result));
    }

    public async Task<Result> ResetPasswordAsync(
        Guid userId,
        string newPassword,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.Failure("User not found.");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(FormatErrors(result));
    }

    public async Task<Result> LockUserAsync(
        Guid userId,
        DateTimeOffset? lockoutEnd,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.Failure("User not found.");

        await _userManager.SetLockoutEnabledAsync(user, true);
        var result = await _userManager.SetLockoutEndDateAsync(
            user,
            lockoutEnd ?? DateTimeOffset.UtcNow.AddYears(100));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(FormatErrors(result));
    }

    public async Task<Result> UnlockUserAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.Failure("User not found.");

        var result = await _userManager.SetLockoutEndDateAsync(user, null);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(FormatErrors(result));
    }

    public async Task<Result<RoleResponse>> CreateRoleAsync(
        string name,
        CancellationToken cancellationToken)
    {
        if (await _roleManager.RoleExistsAsync(name))
            return Result<RoleResponse>.Failure("Role already exists.");

        var role = new ApplicationRole
        {
            Name = name
        };

        var result = await _roleManager.CreateAsync(role);

        return result.Succeeded
            ? Result<RoleResponse>.Success(new RoleResponse(role.Id, role.Name!))
            : Result<RoleResponse>.Failure(FormatErrors(result));
    }

    public async Task<Result<RoleResponse>> UpdateRoleAsync(
        Guid id,
        string name,
        CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role is null)
            return Result<RoleResponse>.Failure("Role not found.");

        var existing = await _roleManager.FindByNameAsync(name);
        if (existing is not null && existing.Id != id)
            return Result<RoleResponse>.Failure("Role already exists.");

        role.Name = name;
        var result = await _roleManager.UpdateAsync(role);

        return result.Succeeded
            ? Result<RoleResponse>.Success(new RoleResponse(role.Id, role.Name!))
            : Result<RoleResponse>.Failure(FormatErrors(result));
    }

    public async Task<Result> DeleteRoleAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role is null)
            return Result.Failure("Role not found.");

        var result = await _roleManager.DeleteAsync(role);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(FormatErrors(result));
    }

    public async Task<Result<RoleResponse>> GetRoleAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var role = await _roleManager.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (role is null)
            return Result<RoleResponse>.Failure("Role not found.");

        return Result<RoleResponse>.Success(
            new RoleResponse(role.Id, role.Name ?? string.Empty));
    }

    public async Task<Result<PagedResponse<RoleResponse>>> GetRolesAsync(
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        IQueryable<ApplicationRole> queryable = _roleManager.Roles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            queryable = queryable.Where(x =>
                x.Name != null && x.Name.ToLower().Contains(search));
        }

        queryable = request.Descending
            ? queryable.OrderByDescending(x => x.Name)
            : queryable.OrderBy(x => x.Name);

        var totalCount = await queryable.CountAsync(cancellationToken);

        var items = await queryable
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new RoleResponse(x.Id, x.Name ?? string.Empty))
            .ToListAsync(cancellationToken);

        return Result<PagedResponse<RoleResponse>>.Success(
            new PagedResponse<RoleResponse>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            });
    }

    public async Task<Result> AssignRoleAsync(
        Guid userId,
        string roleName,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.Failure("User not found.");

        if (!await _roleManager.RoleExistsAsync(roleName))
            return Result.Failure("Role not found.");

        if (await _userManager.IsInRoleAsync(user, roleName))
            return Result.Success();

        var result = await _userManager.AddToRoleAsync(user, roleName);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(FormatErrors(result));
    }

    public async Task<bool> RoleExistsAsync(
        Guid roleId,
        CancellationToken cancellationToken)
    {
        return await _roleManager.Roles
            .AnyAsync(x => x.Id == roleId, cancellationToken);
    }

    private static string FormatErrors(IdentityResult result)
    {
        return string.Join(", ", result.Errors.Select(x => x.Description));
    }
    
}
