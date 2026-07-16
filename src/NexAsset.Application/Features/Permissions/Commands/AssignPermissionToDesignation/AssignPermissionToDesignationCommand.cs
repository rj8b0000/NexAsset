using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Features.Permissions.Commands.AssignPermissionToDesignation;

public sealed record AssignPermissionToDesignationCommand(
    Guid DesignationId,
    Guid PermissionId)
    : IRequest<Result>;

public sealed class AssignPermissionToDesignationCommandHandler
    : IRequestHandler<AssignPermissionToDesignationCommand, Result>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IDesignationRepository _designationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignPermissionToDesignationCommandHandler(
        IPermissionRepository permissionRepository,
        IDesignationRepository designationRepository,
        IUnitOfWork unitOfWork)
    {
        _permissionRepository = permissionRepository;
        _designationRepository = designationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        AssignPermissionToDesignationCommand request,
        CancellationToken cancellationToken)
    {
        var designation = await _designationRepository.GetByIdAsync(
            request.DesignationId,
            cancellationToken);
        if (designation is null)
            return Result.Failure("Designation not found.");

        var permission = await _permissionRepository.GetByIdAsync(
            request.PermissionId,
            cancellationToken);
        if (permission is null)
            return Result.Failure("Permission not found.");

        if (await _permissionRepository.DesignationPermissionExistsAsync(
                request.DesignationId,
                request.PermissionId,
                cancellationToken))
        {
            return Result.Failure("Permission already assigned to designation.");
        }

        await _permissionRepository.AddDesignationPermissionAsync(
            new DesignationPermission
            {
                DesignationId = request.DesignationId,
                PermissionId = request.PermissionId
            },
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
