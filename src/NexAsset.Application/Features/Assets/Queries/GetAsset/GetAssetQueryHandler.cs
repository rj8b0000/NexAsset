using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Assets.Queries.GetAsset;

public sealed class GetAssetQueryHandler : IRequestHandler<GetAssetQuery, Result<AssetResponse>>
{
    private readonly IAssetRepository _repository;
    public GetAssetQueryHandler(IAssetRepository repository) => _repository = repository;

    public async Task<Result<AssetResponse>> Handle(GetAssetQuery request, CancellationToken cancellationToken)
    {
        var asset = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return asset is null
            ? Result<AssetResponse>.Failure("Asset not found.")
            : Result<AssetResponse>.Success(AssetMapper.ToResponse(asset));
    }
}
