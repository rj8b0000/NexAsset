namespace NexAsset.Application.Features.Employees.Commands.UpdateEmployee;

public sealed record UpdateEmployeeResponse(
    Guid Id,
    string EmployeeCode,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive);
