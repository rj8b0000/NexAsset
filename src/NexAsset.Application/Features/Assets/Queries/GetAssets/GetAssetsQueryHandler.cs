using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Assets.Queries.GetAssets;

public sealed class GetAssetsQueryHandler : IRequestHandler<GetAssetsQuery, Result<PagedResponse<AssetListItemResponse>>>
{
    private readonly IAssetRepository _repository;
    public GetAssetsQueryHandler(IAssetRepository repository) => _repository = repository;

    public async Task<Result<PagedResponse<AssetListItemResponse>>> Handle(GetAssetsQuery request, CancellationToken cancellationToken)
    {
        var page = await _repository.GetPagedAsync(request, cancellationToken);
        return Result<PagedResponse<AssetListItemResponse>>.Success(page.Map(AssetMapper.ToListItemResponse));
    }
}
