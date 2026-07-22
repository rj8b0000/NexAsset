using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class ProjectCategory : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;
}
