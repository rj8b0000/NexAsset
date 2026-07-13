using System;

namespace NexAsset.Web.Models.HR
{
    // Wire contracts for /api/employees, mirrored from NexAsset.Application (no project reference).
    // JoiningDate is a DateOnly on the backend; System.Text.Json (de)serializes it as an ISO
    // "yyyy-MM-dd" string, so the form model carries it as a string bound to an <input type=date>.

    public sealed record EmployeeListItem(
        Guid Id, string EmployeeCode, string FirstName, string LastName, string Email,
        Guid OrganizationId, Guid? BranchId, Guid? DepartmentId, Guid? DesignationId,
        DateOnly JoiningDate, int EmploymentStatus, bool IsActive);

    public sealed record EmployeeDetail(
        Guid Id, string EmployeeCode, string FirstName, string LastName, string Email, string? Phone,
        Guid OrganizationId, Guid? BranchId, Guid? DepartmentId, Guid? DesignationId, Guid? ReportingManagerId,
        DateOnly JoiningDate, int EmploymentStatus, Guid? IdentityUserId, bool IsActive);

    /// <summary>
    /// Create/edit form model. On create the backend also requires a <see cref="Password"/> (it
    /// provisions the identity login) and accepts optional role names; neither applies on update.
    /// Required (server-validated): EmployeeCode, FirstName, LastName, Email, OrganizationId,
    /// JoiningDate, EmploymentStatus (and Password on create).
    /// </summary>
    public sealed class EmployeeFormModel
    {
        public Guid? Id { get; set; }
        public string EmployeeCode { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? DesignationId { get; set; }
        public Guid? ReportingManagerId { get; set; }
        public string JoiningDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        public int EmploymentStatus { get; set; } = HR.EmploymentStatus.Active;
        public bool IsActive { get; set; } = true;

        public static EmployeeFormModel FromDetail(EmployeeDetail d) => new()
        {
            Id = d.Id, EmployeeCode = d.EmployeeCode, FirstName = d.FirstName, LastName = d.LastName,
            Email = d.Email, Phone = d.Phone, OrganizationId = d.OrganizationId, BranchId = d.BranchId,
            DepartmentId = d.DepartmentId, DesignationId = d.DesignationId, ReportingManagerId = d.ReportingManagerId,
            JoiningDate = d.JoiningDate.ToString("yyyy-MM-dd"), EmploymentStatus = d.EmploymentStatus, IsActive = d.IsActive
        };
    }
}
