using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.LockUser;

public sealed record LockUserCommand(
    Guid UserId,
    DateTimeOffset? LockoutEnd)
    : IRequest<Result>;
