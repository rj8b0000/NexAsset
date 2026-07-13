namespace NexAsset.Application.Common.Models.Identity;

public sealed record CreateEmployeeUserRequest(
    Guid EmployeeId,
    Guid OrganizationId,
    Guid? BranchId,
    Guid? DepartmentId,
    Guid? DesignationId,
    string Email,
    string Password,
    IReadOnlyCollection<string> Roles);
