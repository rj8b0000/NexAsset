using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.Logout;

public sealed class LogoutCommandHandler
    : IRequestHandler<LogoutCommand, Result>
{
    private readonly IIdentityService _identityService;

    public LogoutCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        return _identityService.LogoutAsync();
    }
}