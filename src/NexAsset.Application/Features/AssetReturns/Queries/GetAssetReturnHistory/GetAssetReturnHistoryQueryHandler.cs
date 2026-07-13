using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.AssetReturns.Queries.GetAssetReturnHistory;

public sealed class GetAssetReturnHistoryQueryHandler
    : IRequestHandler<GetAssetReturnHistoryQuery, Result<IReadOnlyCollection<AssetReturnResponse>>>
{
    private readonly IAssetReturnRepository _repository;
    public GetAssetReturnHistoryQueryHandler(IAssetReturnRepository repository) => _repository = repository;

    public async Task<Result<IReadOnlyCollection<AssetReturnResponse>>> Handle(GetAssetReturnHistoryQuery request, CancellationToken cancellationToken)
    {
        var history = await _repository.GetHistoryByAssetIdAsync(request.AssetId, cancellationToken);
        return Result<IReadOnlyCollection<AssetReturnResponse>>.Success(history.Select(AssetWorkflowMapper.ToReturnResponse).ToList());
    }
}
