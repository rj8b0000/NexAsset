using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Permissions.Commands.RemovePermissionFromRole;

public sealed class RemovePermissionFromRoleCommandHandler
    : IRequestHandler<RemovePermissionFromRoleCommand, Result>
{
    private readonly IPermissionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RemovePermissionFromRoleCommandHandler(
        IPermissionRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        RemovePermissionFromRoleCommand request,
        CancellationToken cancellationToken)
    {
        var rolePermission = await _repository.GetRolePermissionAsync(
            request.RoleId,
            request.PermissionId,
            cancellationToken);

        if (rolePermission is null)
            return Result.Failure("Role permission not found.");

        _repository.RemoveRolePermission(rolePermission);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
