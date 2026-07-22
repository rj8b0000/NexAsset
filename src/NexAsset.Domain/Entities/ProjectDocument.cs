using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class ProjectDocument : BaseEntity
{
    public string DocumentName { get; set; } = default!; // MaxLength(200)
    public DocumentCategory Category { get; set; }
    public string? Description { get; set; }             // MaxLength(500)
    public string FileReference { get; set; } = default!; // stored path/URL, MaxLength(1000)
    public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;
    public int Version { get; set; } = 1;
    public string? Remarks { get; set; }                 // MaxLength(500)
    public DateOnly? ExpiryDate { get; set; }            // For future expiry notifications

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;

    public Guid? UploadedByEmployeeId { get; set; }
    public Employee? UploadedByEmployee { get; set; }
}
