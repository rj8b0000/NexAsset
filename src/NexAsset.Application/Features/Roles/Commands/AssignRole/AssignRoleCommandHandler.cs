using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Roles.Commands.AssignRole;

public sealed class AssignRoleCommandHandler
    : IRequestHandler<AssignRoleCommand, Result>
{
    private readonly IIdentityService _identityService;

    public AssignRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result> Handle(
        AssignRoleCommand request,
        CancellationToken cancellationToken)
    {
        return _identityService.AssignRoleAsync(
            request.UserId,
            request.RoleName,
            cancellationToken);
    }
}
