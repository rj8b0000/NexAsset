using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Employees.Commands.CreateEmployee;

public sealed record CreateEmployeeCommand(
    string EmployeeCode,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string? Phone,
    Guid OrganizationId,
    Guid? BranchId,
    Guid? DepartmentId,
    Guid? DesignationId,
    Guid? ReportingManagerId,
    DateOnly JoiningDate,
    EmploymentStatus EmploymentStatus,
    IReadOnlyCollection<string>? Roles)
    : IRequest<Result<CreateEmployeeResponse>>;
