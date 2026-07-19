using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Users.Commands.RemoveRole;

public sealed class RemoveRoleCommandHandler
    : IRequestHandler<RemoveRoleCommand, Result>
{
    private readonly IIdentityService _identityService;

    public RemoveRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result> Handle(
        RemoveRoleCommand request,
        CancellationToken cancellationToken)
    {
        return _identityService.RemoveRoleAsync(
            request.UserId,
            request.RoleName,
            cancellationToken);
    }
}
