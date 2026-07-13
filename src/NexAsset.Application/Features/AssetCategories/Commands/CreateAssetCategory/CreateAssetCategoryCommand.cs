using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategory;

namespace NexAsset.Application.Features.AssetCategories.Commands.CreateAssetCategory;

public sealed record CreateAssetCategoryCommand(
    Guid OrganizationId,
    string Code,
    string Name,
    string? Description)
    : IRequest<Result<AssetCategoryResponse>>;
