namespace NexAsset.Application.Features.Designations.Queries.GetDesignations;

public sealed record DesignationListItemResponse(
    Guid Id,
    Guid OrganizationId,
    Guid? DepartmentId,
    string Title,
    bool IsActive);
