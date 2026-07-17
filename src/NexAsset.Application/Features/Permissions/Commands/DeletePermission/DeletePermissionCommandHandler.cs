using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Permissions.Commands.DeletePermission;

public sealed class DeletePermissionCommandHandler
    : IRequestHandler<DeletePermissionCommand, Result>
{
    private readonly IPermissionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePermissionCommandHandler(
        IPermissionRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeletePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var permission = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (permission is null)
            return Result.Failure("Permission not found.");

        permission.IsDeleted = true;
        permission.IsActive = false;
        permission.DeletedAtUtc = DateTime.UtcNow;

        _repository.Update(permission);

        // Drop the role/designation mappings too — a deleted permission must not leave grants
        // behind that would silently take effect again if the code were ever recreated.
        await _repository.RemoveMappingsForPermissionAsync(permission.Id, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
