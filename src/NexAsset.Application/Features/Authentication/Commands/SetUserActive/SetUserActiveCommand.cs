using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.SetUserActive;

public sealed record SetUserActiveCommand(
    Guid UserId,
    bool IsActive)
    : IRequest<Result>;
