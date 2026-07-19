using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Users.Commands.RemoveRole;

public sealed record RemoveRoleCommand(
    Guid UserId,
    string RoleName)
    : IRequest<Result>;
