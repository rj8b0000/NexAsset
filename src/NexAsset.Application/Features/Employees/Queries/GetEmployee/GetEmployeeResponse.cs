using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Employees.Queries.GetEmployee;

public sealed record GetEmployeeResponse(
    Guid Id,
    string EmployeeCode,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    Guid OrganizationId,
    Guid? BranchId,
    Guid? DepartmentId,
    Guid? DesignationId,
    Guid? ReportingManagerId,
    DateOnly JoiningDate,
    EmploymentStatus EmploymentStatus,
    Guid? IdentityUserId,
    bool IsActive);
