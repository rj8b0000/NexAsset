using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class Permission : BaseEntity
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
