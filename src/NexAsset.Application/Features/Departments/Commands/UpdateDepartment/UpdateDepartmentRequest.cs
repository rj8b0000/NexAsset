namespace NexAsset.Application.Features.Departments.Commands.UpdateDepartment;

public sealed record UpdateDepartmentRequest(
    Guid OrganizationId,
    string Code,
    string Name,
    string? Description,
    bool IsActive);
