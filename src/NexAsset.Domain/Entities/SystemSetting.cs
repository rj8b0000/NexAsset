using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class SystemSetting : BaseEntity
{
    public Guid? OrganizationId { get; set; }
    public string Key { get; set; } = default!;
    public string Value { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsEncrypted { get; set; }
}
