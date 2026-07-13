namespace NexAsset.Application.Features.Departments.Queries.GetDepartments;

public sealed record DepartmentListItemResponse(
    Guid Id,
    Guid OrganizationId,
    string Code,
    string Name,
    bool IsActive);
