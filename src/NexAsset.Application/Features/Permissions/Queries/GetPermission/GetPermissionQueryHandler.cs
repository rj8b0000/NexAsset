using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Permissions.Queries.GetPermission;

public sealed class GetPermissionQueryHandler
    : IRequestHandler<GetPermissionQuery, Result<PermissionResponse>>
{
    private readonly IPermissionRepository _repository;

    public GetPermissionQueryHandler(IPermissionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PermissionResponse>> Handle(
        GetPermissionQuery request,
        CancellationToken cancellationToken)
    {
        var permission = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (permission is null)
            return Result<PermissionResponse>.Failure("Permission not found.");

        return Result<PermissionResponse>.Success(
            PermissionMapper.ToResponse(permission));
    }
}
