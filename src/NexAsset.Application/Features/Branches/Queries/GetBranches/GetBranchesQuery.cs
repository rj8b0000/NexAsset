using MediatR;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Branches.Queries.GetBranches;

public sealed class GetBranchesQuery
    : PagedRequest, IRequest<Result<PagedResponse<BranchListItemResponse>>>;
