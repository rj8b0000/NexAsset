using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategory;

public sealed record GetAssetCategoryQuery(Guid Id) : IRequest<Result<AssetCategoryResponse>>;
