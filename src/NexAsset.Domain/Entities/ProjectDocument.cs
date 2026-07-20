using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class ProjectDocument : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string DocumentName { get; set; } = default!;
    public string FilePath { get; set; } = default!;
    public Guid UploadedBy { get; set; }
    public Employee UploadedByEmployee { get; set; } = default!;
    public DateTime UploadedOn { get; set; } = DateTime.UtcNow;
    public int Version { get; set; } = 1;
    public DateOnly? ExpiryDate { get; set; }
    public string? Remarks { get; set; }
}
