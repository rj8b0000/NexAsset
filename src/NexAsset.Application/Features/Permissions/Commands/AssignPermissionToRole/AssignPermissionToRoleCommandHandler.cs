using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Features.Permissions.Commands.AssignPermissionToRole;

public sealed class AssignPermissionToRoleCommandHandler
    : IRequestHandler<AssignPermissionToRoleCommand, Result>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;

    public AssignPermissionToRoleCommandHandler(
        IPermissionRepository permissionRepository,
        IIdentityService identityService,
        IUnitOfWork unitOfWork)
    {
        _permissionRepository = permissionRepository;
        _identityService = identityService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        AssignPermissionToRoleCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _identityService.RoleExistsAsync(request.RoleId, cancellationToken))
            return Result.Failure("Role not found.");

        var permission = await _permissionRepository.GetByIdAsync(
            request.PermissionId,
            cancellationToken);

        if (permission is null)
            return Result.Failure("Permission not found.");

        if (await _permissionRepository.RolePermissionExistsAsync(
                request.RoleId,
                request.PermissionId,
                cancellationToken))
        {
            return Result.Failure("Permission already assigned to role.");
        }

        await _permissionRepository.AddRolePermissionAsync(
            new RolePermission
            {
                RoleId = request.RoleId,
                PermissionId = request.PermissionId
            },
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
