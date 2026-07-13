using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.LockUser;

public sealed class LockUserCommandHandler
    : IRequestHandler<LockUserCommand, Result>
{
    private readonly IIdentityService _identityService;

    public LockUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result> Handle(
        LockUserCommand request,
        CancellationToken cancellationToken)
    {
        return _identityService.LockUserAsync(
            request.UserId,
            request.LockoutEnd,
            cancellationToken);
    }
}
