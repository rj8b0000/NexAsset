using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class Designation : BaseEntity
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
}
