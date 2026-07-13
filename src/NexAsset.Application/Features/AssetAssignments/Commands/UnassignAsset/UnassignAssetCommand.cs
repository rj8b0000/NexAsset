using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.AssetAssignments.Commands.UnassignAsset;

public sealed record UnassignAssetCommand(
    Guid AssetId,
    DateOnly UnassignedDate,
    string? Remarks)
    : IRequest<Result>;
