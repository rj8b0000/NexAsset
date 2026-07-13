using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Roles.Commands.AssignRole;

public sealed record AssignRoleCommand(
    Guid UserId,
    string RoleName)
    : IRequest<Result>;
