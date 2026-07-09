using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password,
    bool RememberMe)
    : IRequest<Result<LoginResponse>>;