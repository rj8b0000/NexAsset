using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategories;

public sealed class GetAssetCategoriesQueryHandler
    : IRequestHandler<GetAssetCategoriesQuery, Result<PagedResponse<AssetCategoryListItemResponse>>>
{
    private readonly IAssetCategoryRepository _repository;

    public GetAssetCategoriesQueryHandler(IAssetCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResponse<AssetCategoryListItemResponse>>> Handle(
        GetAssetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var page = await _repository.GetPagedAsync(request, cancellationToken);
        return Result<PagedResponse<AssetCategoryListItemResponse>>
            .Success(page.Map(AssetCategoryMapper.ToListItemResponse));
    }
}
