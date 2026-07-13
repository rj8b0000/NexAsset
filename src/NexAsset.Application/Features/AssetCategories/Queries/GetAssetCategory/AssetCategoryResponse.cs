namespace NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategory;

public sealed record AssetCategoryResponse(
    Guid Id,
    Guid OrganizationId,
    string Code,
    string Name,
    string? Description,
    bool IsActive);
