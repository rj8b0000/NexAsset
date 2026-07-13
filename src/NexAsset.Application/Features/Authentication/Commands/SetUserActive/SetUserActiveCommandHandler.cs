using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.SetUserActive;

public sealed class SetUserActiveCommandHandler
    : IRequestHandler<SetUserActiveCommand, Result>
{
    private readonly IIdentityService _identityService;

    public SetUserActiveCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result> Handle(
        SetUserActiveCommand request,
        CancellationToken cancellationToken)
    {
        return _identityService.SetUserActiveAsync(
            request.UserId,
            request.IsActive,
            cancellationToken);
    }
}
