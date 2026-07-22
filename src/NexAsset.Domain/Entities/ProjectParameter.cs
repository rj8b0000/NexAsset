using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class ProjectParameter : BaseEntity
{
    public string ParameterName { get; set; } = default!;
    public ParameterInputType InputType { get; set; }
    public string? Unit { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public string? DropdownOptionsJson { get; set; }

    public Guid SectionId { get; set; }
    public ProjectParameterSection Section { get; set; } = default!;
}
