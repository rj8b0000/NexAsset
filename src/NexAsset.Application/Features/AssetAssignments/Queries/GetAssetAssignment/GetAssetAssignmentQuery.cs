using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.AssetAssignments.Queries.GetAssetAssignment;

public sealed record GetAssetAssignmentQuery(Guid Id) : IRequest<Result<AssetAssignmentResponse>>;
