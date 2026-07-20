using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class ProjectSetting : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string Value { get; set; } = default!;
    public string? Description { get; set; }
}
