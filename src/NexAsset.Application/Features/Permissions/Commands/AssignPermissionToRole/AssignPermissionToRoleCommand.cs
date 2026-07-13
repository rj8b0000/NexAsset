using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Permissions.Commands.AssignPermissionToRole;

public sealed record AssignPermissionToRoleCommand(
    Guid RoleId,
    Guid PermissionId)
    : IRequest<Result>;
