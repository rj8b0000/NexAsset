namespace NexAsset.Application.Features.AssetCategories.Commands.UpdateAssetCategory;

public sealed record UpdateAssetCategoryRequest(
    Guid OrganizationId,
    string Code,
    string Name,
    string? Description,
    bool IsActive);
