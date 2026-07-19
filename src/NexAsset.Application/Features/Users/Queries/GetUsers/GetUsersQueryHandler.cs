using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, Result<PagedResponse<UserListItemResponse>>>
{
    private readonly IIdentityService _identityService;

    public GetUsersQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result<PagedResponse<UserListItemResponse>>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        return _identityService.GetUsersAsync(
            new PagedRequest
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Search = request.Search,
                SortBy = request.SortBy,
                Descending = request.Descending
            },
            cancellationToken);
    }
}
