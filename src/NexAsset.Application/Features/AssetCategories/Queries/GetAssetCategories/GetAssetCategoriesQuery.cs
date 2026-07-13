using MediatR;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategories;

public sealed class GetAssetCategoriesQuery
    : PagedRequest, IRequest<Result<PagedResponse<AssetCategoryListItemResponse>>>;
