namespace NexAsset.Application.Features.Designations.Queries.GetDesignation;

public sealed record GetDesignationResponse(
    Guid Id,
    Guid OrganizationId,
    Guid? DepartmentId,
    string Title,
    string? Description,
    bool IsActive);
