using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.AssetAssignments.Queries.GetAssetAssignment;

public sealed record AssetAssignmentResponse(
    Guid Id,
    Guid AssetId,
    Guid EmployeeId,
    Guid OrganizationId,
    Guid? BranchId,
    Guid? DepartmentId,
    DateOnly AssignedDate,
    DateOnly? UnassignedDate,
    AssetAssignmentStatus Status,
    string? Remarks);
