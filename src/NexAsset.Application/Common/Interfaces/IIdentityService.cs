using NexAsset.Application.Common.Results;
using NexAsset.Application.Common.Models.Identity;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Features.Authentication.Commands.Login;
using NexAsset.Application.Features.Authentication.Commands.Register;

namespace NexAsset.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<LoginResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken
    );
    Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<Result> LogoutAsync();

    Task<Result<Guid>> CreateEmployeeUserAsync(
        CreateEmployeeUserRequest request,
        CancellationToken cancellationToken);

    Task<Result> UpdateEmployeeUserAsync(
        Guid userId,
        Guid organizationId,
        Guid? branchId,
        Guid? departmentId,
        Guid? designationId,
        string email,
        bool isActive,
        CancellationToken cancellationToken);

    Task<Result> SetUserActiveAsync(
        Guid userId,
        bool isActive,
        CancellationToken cancellationToken);

    Task<Result> ResetPasswordAsync(
        Guid userId,
        string newPassword,
        CancellationToken cancellationToken);

    Task<Result> LockUserAsync(
        Guid userId,
        DateTimeOffset? lockoutEnd,
        CancellationToken cancellationToken);

    Task<Result> UnlockUserAsync(
        Guid userId,
        CancellationToken cancellationToken);

    Task<Result<RoleResponse>> CreateRoleAsync(
        string name,
        CancellationToken cancellationToken);

    Task<Result<RoleResponse>> UpdateRoleAsync(
        Guid id,
        string name,
        CancellationToken cancellationToken);

    Task<Result> DeleteRoleAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<Result<RoleResponse>> GetRoleAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<Result<PagedResponse<RoleResponse>>> GetRolesAsync(
        PagedRequest request,
        CancellationToken cancellationToken);

    Task<Result> AssignRoleAsync(
        Guid userId,
        string roleName,
        CancellationToken cancellationToken);

    Task<bool> RoleExistsAsync(
        Guid roleId,
        CancellationToken cancellationToken);
}
