using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class ProjectParameterValue : BaseEntity
{
    public string? Value { get; set; } // MaxLength(2000)

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;

    public Guid ParameterId { get; set; }
    public ProjectParameter Parameter { get; set; } = default!;
}
