using System;
using System.Collections.Generic;

namespace NexAsset.Web.Models.Users
{
    /// <summary>
    /// A login account. <see cref="EmployeeId"/> decides where the account's permissions come
    /// from: employees draw them from their designation, accounts without an employee record
    /// draw them from their roles, and SuperAdmin bypasses both.
    /// </summary>
    public sealed record UserListItem(
        Guid Id,
        string Email,
        string UserName,
        bool IsActive,
        bool IsLockedOut,
        DateTimeOffset? LockoutEnd,
        DateTime CreatedAtUtc,
        DateTime? LoginAtUtc,
        List<string> Roles,
        Guid? EmployeeId,
        string? EmployeeName,
        string? DesignationTitle,
        string? OrganizationName);

    /// <summary>Create form for a standalone (non-employee) login account.</summary>
    public sealed class UserFormModel
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

        /// <summary>Organization the account may see. Null only for system-wide (SuperAdmin) logins.</summary>
        public Guid? OrganizationId { get; set; }

        public List<string> Roles { get; set; } = new();
    }

    public sealed record CreateUserRequest(
        string Email,
        string Password,
        Guid? OrganizationId,
        IReadOnlyCollection<string> Roles);

    public sealed record AssignRoleRequest(string RoleName);

    public sealed record SetActiveRequest(bool IsActive);

    public sealed record ResetPasswordRequest(string NewPassword);
}
