namespace NexAsset.Application.Features.Designations.Commands.UpdateDesignation;

public sealed record UpdateDesignationRequest(
    Guid OrganizationId,
    Guid? DepartmentId,
    string Title,
    string? Description,
    bool IsActive);
