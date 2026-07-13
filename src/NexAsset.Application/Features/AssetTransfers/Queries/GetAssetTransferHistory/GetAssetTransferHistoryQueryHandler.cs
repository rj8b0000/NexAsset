using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.AssetTransfers.Queries.GetAssetTransferHistory;

public sealed class GetAssetTransferHistoryQueryHandler
    : IRequestHandler<GetAssetTransferHistoryQuery, Result<IReadOnlyCollection<AssetTransferResponse>>>
{
    private readonly IAssetTransferRepository _repository;
    public GetAssetTransferHistoryQueryHandler(IAssetTransferRepository repository) => _repository = repository;

    public async Task<Result<IReadOnlyCollection<AssetTransferResponse>>> Handle(GetAssetTransferHistoryQuery request, CancellationToken cancellationToken)
    {
        var history = await _repository.GetHistoryByAssetIdAsync(request.AssetId, cancellationToken);
        return Result<IReadOnlyCollection<AssetTransferResponse>>.Success(history.Select(AssetWorkflowMapper.ToTransferResponse).ToList());
    }
}
