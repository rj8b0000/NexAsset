using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Permissions.Queries.GetPermission;

namespace NexAsset.Application.Features.Permissions.Queries.GetRolePermissions;

public sealed class GetRolePermissionsQueryHandler
    : IRequestHandler<GetRolePermissionsQuery, Result<IReadOnlyCollection<PermissionResponse>>>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IIdentityService _identityService;

    public GetRolePermissionsQueryHandler(
        IPermissionRepository permissionRepository,
        IIdentityService identityService)
    {
        _permissionRepository = permissionRepository;
        _identityService = identityService;
    }

    public async Task<Result<IReadOnlyCollection<PermissionResponse>>> Handle(
        GetRolePermissionsQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _identityService.RoleExistsAsync(request.RoleId, cancellationToken))
            return Result<IReadOnlyCollection<PermissionResponse>>.Failure("Role not found.");

        var permissions = await _permissionRepository.GetByRoleIdAsync(
            request.RoleId,
            cancellationToken);

        return Result<IReadOnlyCollection<PermissionResponse>>.Success(
            permissions.Select(PermissionMapper.ToResponse).ToList());
    }
}
