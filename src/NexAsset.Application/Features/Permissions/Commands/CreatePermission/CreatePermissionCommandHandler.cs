using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Permissions.Queries.GetPermission;

namespace NexAsset.Application.Features.Permissions.Commands.CreatePermission;

public sealed class CreatePermissionCommandHandler
    : IRequestHandler<CreatePermissionCommand, Result<PermissionResponse>>
{
    private readonly IPermissionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePermissionCommandHandler(
        IPermissionRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PermissionResponse>> Handle(
        CreatePermissionCommand request,
        CancellationToken cancellationToken)
    {
        if (await _repository.ExistsByCodeAsync(request.Code, cancellationToken))
            return Result<PermissionResponse>.Failure("Permission code already exists.");

        var permission = PermissionMapper.ToEntity(request);
        permission.IsActive = true;

        await _repository.AddAsync(permission, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<PermissionResponse>.Success(
            PermissionMapper.ToResponse(permission));
    }
}
