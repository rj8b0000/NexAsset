using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Identity;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Authentication.Commands.Login;
using NexAsset.Application.Features.Authentication.Commands.Register;
using NexAsset.Application.Features.Users.Queries.GetUsers;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Identity.Services;

public sealed class IdentityService : IIdentityService
{
    private const string SuperAdminRole = "SuperAdmin";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly ITenantContext _tenant;

    public IdentityService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
        ITenantContext tenant)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
        _tenant = tenant;
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

    public async Task<Result<PagedResponse<UserListItemResponse>>> GetUsersAsync(
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        IQueryable<ApplicationUser> queryable = _userManager.Users.AsNoTracking();

        // Identity tables carry no query filter of their own, so apply the organization
        // boundary here: an organization administrator only manages their own people.
        if (_tenant.FilterOrganizationId is { } tenantOrganizationId)
            queryable = queryable.Where(x => x.OrganizationId == tenantOrganizationId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            queryable = queryable.Where(x =>
                (x.Email != null && x.Email.ToLower().Contains(search)) ||
                (x.UserName != null && x.UserName.ToLower().Contains(search)));
        }

        queryable = request.Descending
            ? queryable.OrderByDescending(x => x.Email)
            : queryable.OrderBy(x => x.Email);

        var totalCount = await queryable.CountAsync(cancellationToken);

        var users = await queryable
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var userIds = users.Select(x => x.Id).ToList();

        var roles = await (
                from userRole in _context.UserRoles
                join role in _context.Roles on userRole.RoleId equals role.Id
                where userIds.Contains(userRole.UserId)
                select new { userRole.UserId, role.Name })
            .ToListAsync(cancellationToken);

        // The linked employee decides where a user's permissions actually come from.
        var employees = await _context.Employees
            .AsNoTracking()
            .Where(x => x.IdentityUserId != null
                        && userIds.Contains(x.IdentityUserId.Value)
                        && !x.IsDeleted)
            .Select(x => new
            {
                x.Id,
                UserId = x.IdentityUserId!.Value,
                x.FirstName,
                x.LastName,
                DesignationTitle = x.Designation != null ? x.Designation.Title : null,
                OrganizationName = x.Organization.Name
            })
            .ToListAsync(cancellationToken);

        // Accounts without an employee record carry their organization on the login itself.
        var accountOrgIds = users
            .Where(x => x.OrganizationId != null)
            .Select(x => x.OrganizationId!.Value)
            .Distinct()
            .ToList();

        var organizationNames = await _context.Organizations
            .IgnoreQueryFilters()
            .Where(x => accountOrgIds.Contains(x.Id) && !x.IsDeleted)
            .ToDictionaryAsync(x => x.Id, x => x.Name, cancellationToken);

        var now = DateTimeOffset.UtcNow;

        var items = users
            .Select(user =>
            {
                var employee = employees.FirstOrDefault(x => x.UserId == user.Id);
                var organizationName = employee?.OrganizationName;
                if (organizationName is null && user.OrganizationId is { } accountOrgId)
                    organizationNames.TryGetValue(accountOrgId, out organizationName);
                return new UserListItemResponse(
                    user.Id,
                    user.Email ?? string.Empty,
                    user.UserName ?? string.Empty,
                    user.IsActive,
                    user.LockoutEnd.HasValue && user.LockoutEnd > now,
                    user.LockoutEnd,
                    user.CreatedAtUtc,
                    user.LoginAtUtc,
                    roles.Where(x => x.UserId == user.Id)
                        .Select(x => x.Name ?? string.Empty)
                        .OrderBy(x => x)
                        .ToList(),
                    employee?.Id,
                    employee is null ? null : $"{employee.FirstName} {employee.LastName}",
                    employee?.DesignationTitle,
                    organizationName);
            })
            .ToList();

        return Result<PagedResponse<UserListItemResponse>>.Success(
            new PagedResponse<UserListItemResponse>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            });
    }

    public async Task<Result<Guid>> CreateUserAsync(
        string email,
        string password,
        Guid? organizationId,
        IReadOnlyCollection<string> roles,
        CancellationToken cancellationToken)
    {
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing is not null)
            return Result<Guid>.Failure("Email already registered");

        if (organizationId is { } orgId
            && !await _context.Organizations.AnyAsync(x => x.Id == orgId && !x.IsDeleted, cancellationToken))
        {
            return Result<Guid>.Failure("Organization not found.");
        }

        var requestedRoles = roles?.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList() ?? [];
        foreach (var role in requestedRoles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                return Result<Guid>.Failure($"Role '{role}' not found.");
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            IsActive = true,
            // Decides which organization's data this login may read (see ITenantContext).
            OrganizationId = organizationId
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return Result<Guid>.Failure(FormatErrors(result));

        foreach (var role in requestedRoles)
        {
            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
                return Result<Guid>.Failure(FormatErrors(roleResult));
        }

        return Result<Guid>.Success(user.Id);
    }

    public async Task<Result> RemoveRoleAsync(
        Guid userId,
        string roleName,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.Failure("User not found.");

        if (!await _userManager.IsInRoleAsync(user, roleName))
            return Result.Success();

        // Losing the last SuperAdmin would leave nobody able to administer the system.
        if (string.Equals(roleName, SuperAdminRole, StringComparison.OrdinalIgnoreCase))
        {
            var superAdmins = await _userManager.GetUsersInRoleAsync(SuperAdminRole);
            if (superAdmins.Count <= 1)
                return Result.Failure("Cannot remove the last SuperAdmin.");
        }

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(FormatErrors(result));
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
