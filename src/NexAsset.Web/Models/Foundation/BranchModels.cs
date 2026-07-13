using System;

namespace NexAsset.Web.Models.Foundation
{
    // Wire contracts for /api/branches. A branch belongs to an organization (OrganizationId).

    public sealed record BranchListItem(Guid Id, Guid OrganizationId, string Code, string Name, string? Email, bool IsActive);

    public sealed record BranchDetail(
        Guid Id, Guid OrganizationId, string Code, string Name, string? Email, string? Phone,
        string? Address, string? City, string? State, string? Country, string? PostalCode, bool IsActive);

    /// <summary>Create/edit form model. Required (server-validated): OrganizationId, Code, Name. Email must be valid if provided.</summary>
    public sealed class BranchFormModel
    {
        public Guid? Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public bool IsActive { get; set; } = true;

        public static BranchFormModel FromDetail(BranchDetail d) => new()
        {
            Id = d.Id, OrganizationId = d.OrganizationId, Code = d.Code, Name = d.Name, Email = d.Email,
            Phone = d.Phone, Address = d.Address, City = d.City, State = d.State, Country = d.Country,
            PostalCode = d.PostalCode, IsActive = d.IsActive
        };
    }
}
