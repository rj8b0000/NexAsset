using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Permissions.Queries.GetPermissions;

public sealed class GetPermissionsQueryHandler
    : IRequestHandler<GetPermissionsQuery, Result<PagedResponse<PermissionListItemResponse>>>
{
    private readonly IPermissionRepository _repository;

    public GetPermissionsQueryHandler(IPermissionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResponse<PermissionListItemResponse>>> Handle(
        GetPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var page = await _repository.GetPagedAsync(request, cancellationToken);

        return Result<PagedResponse<PermissionListItemResponse>>
            .Success(page.Map(PermissionMapper.ToListItemResponse));
    }
}
