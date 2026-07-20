using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class Project : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;
    public Guid CategoryId { get; set; }
    public ProjectCategory Category { get; set; } = default!;
    public Guid? ClientId { get; set; }
    public Customer? Client { get; set; }
    public Guid BranchId { get; set; }
    public Branch Branch { get; set; } = default!;
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = default!;
    public Guid ProjectManagerId { get; set; }
    public Employee ProjectManager { get; set; } = default!;
    public string ProjectName { get; set; } = default!;
    public string? Description { get; set; }
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    public ProjectStatus Status { get; set; } = ProjectStatus.Draft;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public DateOnly? ExpectedCompletion { get; set; }
    public string? Notes { get; set; }
    public bool IsArchived { get; set; }
    public DateTime? ArchivedAtUtc { get; set; }
}
