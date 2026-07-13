using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategory;

public sealed class GetAssetCategoryQueryHandler
    : IRequestHandler<GetAssetCategoryQuery, Result<AssetCategoryResponse>>
{
    private readonly IAssetCategoryRepository _repository;

    public GetAssetCategoryQueryHandler(IAssetCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<AssetCategoryResponse>> Handle(GetAssetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return category is null
            ? Result<AssetCategoryResponse>.Failure("Asset category not found.")
            : Result<AssetCategoryResponse>.Success(AssetCategoryMapper.ToResponse(category));
    }
}
