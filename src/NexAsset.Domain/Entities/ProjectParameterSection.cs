using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class ProjectParameterSection : BaseEntity
{
    public string Name { get; set; } = default!;
    public int DisplayOrder { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;

    public ICollection<ProjectParameter> Parameters { get; set; } = new List<ProjectParameter>();
}
