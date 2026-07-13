using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Branches.Queries.GetBranch;

public sealed class GetBranchQueryHandler
    : IRequestHandler<GetBranchQuery, Result<GetBranchResponse>>
{
    private readonly IBranchRepository _repository;

    public GetBranchQueryHandler(
        IBranchRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetBranchResponse>> Handle(
        GetBranchQuery request,
        CancellationToken cancellationToken)
    {
        var branch = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (branch is null)
        {
            return Result<GetBranchResponse>
                .Failure("Branch not found.");
        }

        return Result<GetBranchResponse>.Success(
            BranchMapper.ToGetResponse(branch));
    }
}
