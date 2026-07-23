using System;

namespace NexAsset.Web.Models.Projects;

public class ProjectDocumentItem
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public DocumentCategory Category { get; set; }
    public string? Description { get; set; }
    public string FileReference { get; set; } = string.Empty;
    public DateTime UploadedAtUtc { get; set; }
    public int Version { get; set; } = 1;
    public string? Remarks { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public Guid? UploadedByEmployeeId { get; set; }
    public string? UploadedByEmployeeName { get; set; }
}
