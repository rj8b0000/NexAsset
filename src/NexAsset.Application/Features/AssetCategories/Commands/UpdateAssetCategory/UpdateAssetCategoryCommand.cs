using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategory;

namespace NexAsset.Application.Features.AssetCategories.Commands.UpdateAssetCategory;

public sealed record UpdateAssetCategoryCommand(
    Guid Id,
    Guid OrganizationId,
    string Code,
    string Name,
    string? Description,
    bool IsActive)
    : IRequest<Result<AssetCategoryResponse>>;
