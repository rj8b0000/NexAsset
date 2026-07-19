namespace NexAsset.Application.Features.Users.Queries.GetUsers;

/// <summary>
/// A login account as shown on the Users screen. The employee fields matter for access:
/// permissions come from the linked employee's designation, and roles only apply to accounts
/// with no employee record (SuperAdmin always bypasses).
/// </summary>
public sealed record UserListItemResponse(
    Guid Id,
    string Email,
    string UserName,
    bool IsActive,
    bool IsLockedOut,
    DateTimeOffset? LockoutEnd,
    DateTime CreatedAtUtc,
    DateTime? LoginAtUtc,
    IReadOnlyCollection<string> Roles,
    Guid? EmployeeId,
    string? EmployeeName,
    string? DesignationTitle,
    string? OrganizationName);
