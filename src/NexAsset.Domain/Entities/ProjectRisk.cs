using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class ProjectRisk : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string Probability { get; set; } = "Medium";
    public string Impact { get; set; } = "Medium";
    public string Severity { get; set; } = "Medium";
    public string? MitigationPlan { get; set; }
    public Guid? OwnerEmployeeId { get; set; }
    public Employee? OwnerEmployee { get; set; }
    public string Status { get; set; } = "Open";
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedDate { get; set; }
}
