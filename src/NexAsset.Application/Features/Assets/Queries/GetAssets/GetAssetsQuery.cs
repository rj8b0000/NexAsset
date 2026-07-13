using MediatR;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Assets.Queries.GetAssets;

public sealed class GetAssetsQuery : PagedRequest, IRequest<Result<PagedResponse<AssetListItemResponse>>>;
