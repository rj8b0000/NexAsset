using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Branches.Queries.GetBranches;

public sealed class GetBranchesQueryHandler
    : IRequestHandler<GetBranchesQuery, Result<PagedResponse<BranchListItemResponse>>>
{
    private readonly IBranchRepository _repository;

    public GetBranchesQueryHandler(
        IBranchRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResponse<BranchListItemResponse>>> Handle(
        GetBranchesQuery request,
        CancellationToken cancellationToken)
    {
        var page = await _repository.GetPagedAsync(
            request,
            cancellationToken);

        return Result<PagedResponse<BranchListItemResponse>>
            .Success(page.Map(BranchMapper.ToListItemResponse));
    }
}
