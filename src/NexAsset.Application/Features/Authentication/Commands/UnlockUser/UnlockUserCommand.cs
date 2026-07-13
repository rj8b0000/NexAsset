using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.UnlockUser;

public sealed record UnlockUserCommand(Guid UserId)
    : IRequest<Result>;
