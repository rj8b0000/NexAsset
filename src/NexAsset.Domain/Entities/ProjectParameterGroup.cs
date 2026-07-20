using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class ProjectParameterGroup : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public string GroupName { get; set; } = default!;
    public int DisplayOrder { get; set; }
}
