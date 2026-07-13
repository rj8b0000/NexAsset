namespace NexAsset.Application.Features.Departments.Commands.UpdateDepartment;

public sealed record UpdateDepartmentResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive);
