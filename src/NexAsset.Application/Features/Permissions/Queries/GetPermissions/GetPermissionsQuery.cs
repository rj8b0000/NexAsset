using MediatR;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Permissions.Queries.GetPermissions;

public sealed class GetPermissionsQuery
    : PagedRequest, IRequest<Result<PagedResponse<PermissionListItemResponse>>>;
