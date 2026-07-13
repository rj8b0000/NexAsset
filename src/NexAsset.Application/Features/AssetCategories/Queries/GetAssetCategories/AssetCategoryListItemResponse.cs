namespace NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategories;

public sealed record AssetCategoryListItemResponse(
    Guid Id,
    Guid OrganizationId,
    string Code,
    string Name,
    bool IsActive);
