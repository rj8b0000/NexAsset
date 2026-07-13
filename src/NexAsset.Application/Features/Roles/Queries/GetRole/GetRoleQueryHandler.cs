using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Identity;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Roles.Queries.GetRole;

public sealed class GetRoleQueryHandler
    : IRequestHandler<GetRoleQuery, Result<RoleResponse>>
{
    private readonly IIdentityService _identityService;

    public GetRoleQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result<RoleResponse>> Handle(
        GetRoleQuery request,
        CancellationToken cancellationToken)
    {
        return _identityService.GetRoleAsync(request.Id, cancellationToken);
    }
}
