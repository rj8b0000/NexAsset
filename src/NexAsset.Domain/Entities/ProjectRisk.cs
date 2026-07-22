using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class ProjectRisk : BaseEntity
{
    public string Title { get; set; } = default!;        // MaxLength(200)
    public string? Description { get; set; }             // MaxLength(1000)
    public RiskCategory Category { get; set; }
    public RiskProbability Probability { get; set; }
    public RiskImpact Impact { get; set; }
    public RiskSeverity Severity { get; set; }           // Computed — see matrix
    public RiskStatus Status { get; set; } = RiskStatus.Open;
    public string? MitigationPlan { get; set; }          // MaxLength(1000)
    public string? Remarks { get; set; }                 // MaxLength(500)
    public DateTime? ClosedAtUtc { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;

    public Guid? OwnerEmployeeId { get; set; }
    public Employee? OwnerEmployee { get; set; }
}
