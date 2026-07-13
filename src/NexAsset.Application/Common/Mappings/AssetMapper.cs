using NexAsset.Application.Features.Assets.Commands.CreateAsset;
using NexAsset.Application.Features.Assets.Commands.UpdateAsset;
using NexAsset.Application.Features.Assets.Queries.GetAsset;
using NexAsset.Application.Features.Assets.Queries.GetAssets;
using NexAsset.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class AssetMapper
{
    [MapperIgnoreTarget(nameof(Asset.Id))]
    [MapperIgnoreTarget(nameof(Asset.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Asset.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Asset.IsDeleted))]
    [MapperIgnoreTarget(nameof(Asset.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Asset.Organization))]
    [MapperIgnoreTarget(nameof(Asset.Category))]
    [MapperIgnoreTarget(nameof(Asset.Branch))]
    [MapperIgnoreTarget(nameof(Asset.Department))]
    [MapperIgnoreTarget(nameof(Asset.CurrentEmployee))]
    public static partial Asset ToEntity(CreateAssetCommand command);

    public static partial AssetResponse ToResponse(Asset asset);
    public static partial AssetListItemResponse ToListItemResponse(Asset asset);

    [MapperIgnoreTarget(nameof(Asset.Id))]
    [MapperIgnoreTarget(nameof(Asset.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Asset.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Asset.IsDeleted))]
    [MapperIgnoreTarget(nameof(Asset.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Asset.Organization))]
    [MapperIgnoreTarget(nameof(Asset.Category))]
    [MapperIgnoreTarget(nameof(Asset.Branch))]
    [MapperIgnoreTarget(nameof(Asset.Department))]
    [MapperIgnoreTarget(nameof(Asset.CurrentEmployee))]
    [MapperIgnoreSource(nameof(UpdateAssetCommand.Id))]
    public static partial void ApplyUpdate(UpdateAssetCommand command, Asset asset);
}
