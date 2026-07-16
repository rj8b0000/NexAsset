using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Permissions.Commands.RemovePermissionFromDesignation;

public sealed record RemovePermissionFromDesignationCommand(
    Guid DesignationId,
    Guid PermissionId)
    : IRequest<Result>;

public sealed class RemovePermissionFromDesignationCommandHandler
    : IRequestHandler<RemovePermissionFromDesignationCommand, Result>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemovePermissionFromDesignationCommandHandler(
        IPermissionRepository permissionRepository,
        IUnitOfWork unitOfWork)
    {
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        RemovePermissionFromDesignationCommand request,
        CancellationToken cancellationToken)
    {
        var mapping = await _permissionRepository.GetDesignationPermissionAsync(
            request.DesignationId,
            request.PermissionId,
            cancellationToken);
        if (mapping is null)
            return Result.Failure("Permission is not assigned to this designation.");

        _permissionRepository.RemoveDesignationPermission(mapping);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
