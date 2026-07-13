using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Employees.Commands.UpdateEmployee;

public sealed record UpdateEmployeeRequest(
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
    bool IsActive);
