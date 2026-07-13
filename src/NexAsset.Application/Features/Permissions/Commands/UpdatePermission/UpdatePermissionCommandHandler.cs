using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Permissions.Queries.GetPermission;

namespace NexAsset.Application.Features.Permissions.Commands.UpdatePermission;

public sealed class UpdatePermissionCommandHandler
    : IRequestHandler<UpdatePermissionCommand, Result<PermissionResponse>>
{
    private readonly IPermissionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePermissionCommandHandler(
        IPermissionRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PermissionResponse>> Handle(
        UpdatePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var permission = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (permission is null)
            return Result<PermissionResponse>.Failure("Permission not found.");

        if (await _repository.ExistsByCodeAsync(request.Code, request.Id, cancellationToken))
            return Result<PermissionResponse>.Failure("Permission code already exists.");

        PermissionMapper.ApplyUpdate(request, permission);
        permission.UpdatedAtUtc = DateTime.UtcNow;

        _repository.Update(permission);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<PermissionResponse>.Success(
            PermissionMapper.ToResponse(permission));
    }
}
