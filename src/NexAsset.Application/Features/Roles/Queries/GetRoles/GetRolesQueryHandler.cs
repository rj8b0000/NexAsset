using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Identity;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Roles.Queries.GetRoles;

public sealed class GetRolesQueryHandler
    : IRequestHandler<GetRolesQuery, Result<PagedResponse<RoleResponse>>>
{
    private readonly IIdentityService _identityService;

    public GetRolesQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result<PagedResponse<RoleResponse>>> Handle(
        GetRolesQuery request,
        CancellationToken cancellationToken)
    {
        return _identityService.GetRolesAsync(request, cancellationToken);
    }
}
