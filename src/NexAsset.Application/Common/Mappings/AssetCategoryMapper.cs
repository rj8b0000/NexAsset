using NexAsset.Application.Features.AssetCategories.Commands.CreateAssetCategory;
using NexAsset.Application.Features.AssetCategories.Commands.UpdateAssetCategory;
using NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategories;
using NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategory;
using NexAsset.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class AssetCategoryMapper
{
    [MapperIgnoreTarget(nameof(AssetCategory.Id))]
    [MapperIgnoreTarget(nameof(AssetCategory.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(AssetCategory.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(AssetCategory.IsDeleted))]
    [MapperIgnoreTarget(nameof(AssetCategory.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(AssetCategory.Organization))]
    public static partial AssetCategory ToEntity(CreateAssetCategoryCommand command);

    public static partial AssetCategoryResponse ToResponse(AssetCategory category);
    public static partial AssetCategoryListItemResponse ToListItemResponse(AssetCategory category);

    [MapperIgnoreTarget(nameof(AssetCategory.Id))]
    [MapperIgnoreTarget(nameof(AssetCategory.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(AssetCategory.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(AssetCategory.IsDeleted))]
    [MapperIgnoreTarget(nameof(AssetCategory.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(AssetCategory.Organization))]
    [MapperIgnoreSource(nameof(UpdateAssetCategoryCommand.Id))]
    public static partial void ApplyUpdate(UpdateAssetCategoryCommand command, AssetCategory category);
}
