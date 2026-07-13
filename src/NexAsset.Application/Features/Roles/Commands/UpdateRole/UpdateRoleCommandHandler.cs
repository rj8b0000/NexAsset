using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Identity;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommandHandler
    : IRequestHandler<UpdateRoleCommand, Result<RoleResponse>>
{
    private readonly IIdentityService _identityService;

    public UpdateRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result<RoleResponse>> Handle(
        UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        return _identityService.UpdateRoleAsync(
            request.Id,
            request.Name,
            cancellationToken);
    }
}
