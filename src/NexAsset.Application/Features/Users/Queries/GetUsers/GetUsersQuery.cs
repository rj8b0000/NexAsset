using MediatR;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Search = null,
    string? SortBy = null,
    bool Descending = false)
    : IRequest<Result<PagedResponse<UserListItemResponse>>>;
