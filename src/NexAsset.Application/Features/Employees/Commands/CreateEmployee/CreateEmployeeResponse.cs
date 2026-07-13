namespace NexAsset.Application.Features.Employees.Commands.CreateEmployee;

public sealed record CreateEmployeeResponse(
    Guid Id,
    string EmployeeCode,
    string FirstName,
    string LastName,
    string Email,
    Guid? IdentityUserId);
