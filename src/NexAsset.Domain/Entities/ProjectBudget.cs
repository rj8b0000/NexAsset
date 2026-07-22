using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

// Append-only: never UPDATE a row; INSERT a new version on every change.
public class ProjectBudget : BaseEntity
{
    public decimal EstimatedBudget { get; set; }     // >= 0, decimal(18,2)
    public decimal ApprovedBudget { get; set; }      // >= 0, decimal(18,2)
    public decimal ActualCost { get; set; }          // >= 0, decimal(18,2)
    public decimal ProcurementCost { get; set; }     // >= 0, decimal(18,2)
    public decimal MaintenanceCost { get; set; }     // >= 0, decimal(18,2)
    public decimal LabourCost { get; set; }          // >= 0, decimal(18,2)
    public decimal MiscellaneousCost { get; set; }   // >= 0, decimal(18,2)
    public Guid UpdatedByUserId { get; set; }        // Who made this version

    // Computed (in-memory, not stored):
    // RemainingBudget = ApprovedBudget - ActualCost
    // BudgetPercentageUsed = ApprovedBudget > 0 ? (ActualCost / ApprovedBudget) * 100 : 0
    // BudgetStatus = Under/On/Over Budget

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;

    // Extension point: Finance module integration (nullable FK)
    public Guid? FinanceInvoiceId { get; set; }
}
