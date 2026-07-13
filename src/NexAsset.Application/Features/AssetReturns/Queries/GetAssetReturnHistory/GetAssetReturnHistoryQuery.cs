using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.AssetReturns.Queries.GetAssetReturnHistory;

public sealed record GetAssetReturnHistoryQuery(Guid AssetId)
    : IRequest<Result<IReadOnlyCollection<AssetReturnResponse>>>;
