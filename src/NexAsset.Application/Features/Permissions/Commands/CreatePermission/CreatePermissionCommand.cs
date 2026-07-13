using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Permissions.Queries.GetPermission;

namespace NexAsset.Application.Features.Permissions.Commands.CreatePermission;

public sealed record CreatePermissionCommand(
    string Code,
    string Name,
    string? Description)
    : IRequest<Result<PermissionResponse>>;
