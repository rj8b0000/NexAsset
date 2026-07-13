using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Employees.Queries.GetEmployees;

public sealed record EmployeeListItemResponse(
    Guid Id,
    string EmployeeCode,
    string FirstName,
    string LastName,
    string Email,
    Guid OrganizationId,
    Guid? BranchId,
    Guid? DepartmentId,
    Guid? DesignationId,
    DateOnly JoiningDate,
    EmploymentStatus EmploymentStatus,
    bool IsActive);
