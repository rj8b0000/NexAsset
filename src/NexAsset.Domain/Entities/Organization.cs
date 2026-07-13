using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class Organization:BaseEntity
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string LegalName { get; set; } = default!;
    public string? RegistrationNumber { get; set; }
    public string? TaxNumber { get; set; }
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public string? Website {get; set;}
    public string? LogoUrl { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string TimeZone { get; set; } = "UTC";
    public string Currency { get; set; } = "INR";
    public bool IsActive { get; set; } = true;
}
