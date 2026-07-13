using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class Customer : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? ContactPerson { get; set; }
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
}
