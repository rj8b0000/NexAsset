using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class ProjectBudget : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public decimal EstimatedBudget { get; set; }
    public decimal ActualCost { get; set; }
    public decimal RemainingBudget => EstimatedBudget - ActualCost;
    public decimal ProcurementCost { get; set; }
    public decimal MaintenanceCost { get; set; }
    public decimal LabourCost { get; set; }
    public decimal MiscellaneousCost { get; set; }
}
