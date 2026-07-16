using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Permissions.Queries.GetPermission;

namespace NexAsset.Application.Features.Permissions.Queries.GetDesignationPermissions;

public sealed record GetDesignationPermissionsQuery(Guid DesignationId)
    : IRequest<Result<IReadOnlyCollection<PermissionResponse>>>;

public sealed class GetDesignationPermissionsQueryHandler
    : IRequestHandler<GetDesignationPermissionsQuery, Result<IReadOnlyCollection<PermissionResponse>>>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IDesignationRepository _designationRepository;

    public GetDesignationPermissionsQueryHandler(
        IPermissionRepository permissionRepository,
        IDesignationRepository designationRepository)
    {
        _permissionRepository = permissionRepository;
        _designationRepository = designationRepository;
    }

    public async Task<Result<IReadOnlyCollection<PermissionResponse>>> Handle(
        GetDesignationPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var designation = await _designationRepository.GetByIdAsync(
            request.DesignationId,
            cancellationToken);
        if (designation is null)
            return Result<IReadOnlyCollection<PermissionResponse>>.Failure("Designation not found.");

        var permissions = await _permissionRepository.GetByDesignationIdAsync(
            request.DesignationId,
            cancellationToken);

        return Result<IReadOnlyCollection<PermissionResponse>>.Success(
            permissions
                .Select(p => new PermissionResponse(p.Id, p.Code, p.Name, p.Description, p.IsActive))
                .ToList());
    }
}
