using MediatR;
using NexAsset.Application.Common.Models.Identity;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Roles.Queries.GetRoles;

public sealed class GetRolesQuery
    : PagedRequest, IRequest<Result<PagedResponse<RoleResponse>>>;
