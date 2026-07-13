using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.AssetAssignments.Queries.GetAssetAssignment;

namespace NexAsset.Application.Features.AssetAssignments.Queries.GetAssetAssignmentHistory;

public sealed record GetAssetAssignmentHistoryQuery(Guid AssetId)
    : IRequest<Result<IReadOnlyCollection<AssetAssignmentResponse>>>;
