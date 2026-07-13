using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Permissions.Commands.DeletePermission;

public sealed record DeletePermissionCommand(Guid Id)
    : IRequest<Result>;
