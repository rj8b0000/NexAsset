using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.UnlockUser;

public sealed class UnlockUserCommandHandler
    : IRequestHandler<UnlockUserCommand, Result>
{
    private readonly IIdentityService _identityService;

    public UnlockUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result> Handle(
        UnlockUserCommand request,
        CancellationToken cancellationToken)
    {
        return _identityService.UnlockUserAsync(
            request.UserId,
            cancellationToken);
    }
}
