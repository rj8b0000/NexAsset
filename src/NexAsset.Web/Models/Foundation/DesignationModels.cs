using System;

namespace NexAsset.Web.Models.Foundation
{
    // Wire contracts for /api/designations. Belongs to an organization; optionally to a department.

    public sealed record DesignationListItem(Guid Id, Guid OrganizationId, Guid? DepartmentId, string Title, bool IsActive);

    public sealed record DesignationDetail(Guid Id, Guid OrganizationId, Guid? DepartmentId, string Title, string? Description, bool IsActive);

    /// <summary>Create/edit form model. Required (server-validated): OrganizationId, Title.</summary>
    public sealed class DesignationFormModel
    {
        public Guid? Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public static DesignationFormModel FromDetail(DesignationDetail d) => new()
        {
            Id = d.Id, OrganizationId = d.OrganizationId, DepartmentId = d.DepartmentId,
            Title = d.Title, Description = d.Description, IsActive = d.IsActive
        };
    }
}
