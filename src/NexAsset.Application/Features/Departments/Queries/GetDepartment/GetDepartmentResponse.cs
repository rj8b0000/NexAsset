namespace NexAsset.Application.Features.Departments.Queries.GetDepartment;

public sealed record GetDepartmentResponse(
    Guid Id,
    Guid OrganizationId,
    string Code,
    string Name,
    string? Description,
    bool IsActive);
