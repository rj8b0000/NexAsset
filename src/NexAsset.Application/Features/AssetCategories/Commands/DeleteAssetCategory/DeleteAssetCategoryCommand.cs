using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.AssetCategories.Commands.DeleteAssetCategory;

public sealed record DeleteAssetCategoryCommand(Guid Id) : IRequest<Result>;
