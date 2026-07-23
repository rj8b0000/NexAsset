using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class ProjectDocument : BaseEntity
{
    public string DocumentName { get; set; } = default!;
    public DocumentCategory Category { get; set; }
    public string? Description { get; set; }
    public string FileReference { get; set; } = default!;
    public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;
    public int Version { get; set; } = 1;
    public string? Remarks { get; set; }
    public DateOnly? ExpiryDate { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;

    public Guid? UploadedByEmployeeId { get; set; }
    public Employee? UploadedByEmployee { get; set; }
}
