using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Permissions.Queries.GetPermission;

namespace NexAsset.Application.Features.Permissions.Queries.GetRolePermissions;

public sealed record GetRolePermissionsQuery(Guid RoleId)
    : IRequest<Result<IReadOnlyCollection<PermissionResponse>>>;
