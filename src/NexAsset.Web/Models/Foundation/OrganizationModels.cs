using System;

namespace NexAsset.Web.Models.Foundation
{
    // Wire contracts for /api/organizations, hand-mirrored from NexAsset.Application
    // (no project reference). Keep in sync with the backend records.

    /// <summary>Row shape from GET /api/organizations (paged list).</summary>
    public sealed record OrganizationListItem(Guid Id, string Code, string Name, string Email, bool IsActive);

    /// <summary>
    /// Full record from GET /api/organizations/{id}. NOTE: the backend's GetOrganizationResponse
    /// does NOT include PostalCode even though create/update accept it — see the mismatch note in
    /// the phase report. PostalCode therefore starts blank when editing.
    /// </summary>
    public sealed record OrganizationDetail(
        Guid Id, string Code, string Name, string LegalName, string Email,
        string? Phone, string? Website, string? Address, string? City, string? State,
        string? Country, string Currency, string TimeZone, bool IsActive);

    /// <summary>
    /// Mutable form model bound by the create/edit dialog. Serialized directly as the request
    /// body: create ignores <see cref="IsActive"/>; update includes it. Required (server-validated):
    /// Code, Name, LegalName, Email, Currency, TimeZone.
    /// </summary>
    public sealed class OrganizationFormModel
    {
        public Guid? Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string LegalName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string Currency { get; set; } = "USD";
        public string TimeZone { get; set; } = "UTC";
        public bool IsActive { get; set; } = true;

        public static OrganizationFormModel FromDetail(OrganizationDetail d) => new()
        {
            Id = d.Id, Code = d.Code, Name = d.Name, LegalName = d.LegalName, Email = d.Email,
            Phone = d.Phone, Website = d.Website, Address = d.Address, City = d.City, State = d.State,
            Country = d.Country, Currency = d.Currency, TimeZone = d.TimeZone, IsActive = d.IsActive
        };
    }
}
