using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.AssetAssignments.Queries.GetAssetAssignment;

namespace NexAsset.Application.Features.AssetAssignments.Commands.AssignAsset;

public sealed record AssignAssetCommand(
    Guid AssetId,
    Guid EmployeeId,
    DateOnly AssignedDate,
    string? Remarks)
    : IRequest<Result<AssetAssignmentResponse>>;
