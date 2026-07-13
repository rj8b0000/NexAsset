using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Permissions.Queries.GetPermission;

namespace NexAsset.Application.Features.Permissions.Commands.UpdatePermission;

public sealed record UpdatePermissionCommand(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive)
    : IRequest<Result<PermissionResponse>>;
