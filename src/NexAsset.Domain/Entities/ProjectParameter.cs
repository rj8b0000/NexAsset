using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class ProjectParameter : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public Guid GroupId { get; set; }
    public ProjectParameterGroup Group { get; set; } = default!;
    public string ParameterName { get; set; } = default!;
    public ProjectParameterInputType InputType { get; set; } = ProjectParameterInputType.Text;
    public string? Value { get; set; }
    public string? Unit { get; set; }
    public bool Required { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsVisible { get; set; } = true;
}
