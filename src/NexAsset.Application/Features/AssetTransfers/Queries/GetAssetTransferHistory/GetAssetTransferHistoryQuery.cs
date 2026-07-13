using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.AssetTransfers.Queries.GetAssetTransferHistory;

public sealed record GetAssetTransferHistoryQuery(Guid AssetId)
    : IRequest<Result<IReadOnlyCollection<AssetTransferResponse>>>;
