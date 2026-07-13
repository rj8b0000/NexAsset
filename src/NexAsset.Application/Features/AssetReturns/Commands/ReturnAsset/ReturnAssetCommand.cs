using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.AssetReturns.Queries.GetAssetReturnHistory;

namespace NexAsset.Application.Features.AssetReturns.Commands.ReturnAsset;

public sealed record ReturnAssetCommand(
    Guid AssetId,
    DateOnly ReturnDate,
    string? InspectionNotes,
    string? ReturnRemarks,
    bool IsAssetUsable)
    : IRequest<Result<AssetReturnResponse>>;
