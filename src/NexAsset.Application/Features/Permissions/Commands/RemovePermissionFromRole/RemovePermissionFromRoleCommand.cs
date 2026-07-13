using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Permissions.Commands.RemovePermissionFromRole;

public sealed record RemovePermissionFromRoleCommand(
    Guid RoleId,
    Guid PermissionId)
    : IRequest<Result>;
