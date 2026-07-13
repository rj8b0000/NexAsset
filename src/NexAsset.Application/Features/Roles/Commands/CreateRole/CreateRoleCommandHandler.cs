using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Identity;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandHandler
    : IRequestHandler<CreateRoleCommand, Result<RoleResponse>>
{
    private readonly IIdentityService _identityService;

    public CreateRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result<RoleResponse>> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        return _identityService.CreateRoleAsync(request.Name, cancellationToken);
    }
}
