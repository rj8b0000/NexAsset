namespace NexAsset.Application.Features.Departments.Commands.CreateDepartment;

public sealed record CreateDepartmentResponse(
    Guid Id,
    string Code,
    string Name);
