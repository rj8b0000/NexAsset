using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Assets.Queries.GetAsset;

public sealed record GetAssetQuery(Guid Id) : IRequest<Result<AssetResponse>>;
