using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class Project : BaseEntity
{
    public string ProjectCode { get; set; } = default!;
    public string ProjectName { get; set; } = default!;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? InternalRemarks { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Draft;
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public DateOnly? ExpectedCompletionDate { get; set; }

    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public Guid CategoryId { get; set; }
    public ProjectCategory Category { get; set; } = default!;

    public Guid? BranchId { get; set; }
    public Branch? Branch { get; set; }

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public Guid? ProjectManagerEmployeeId { get; set; }
    public Employee? ProjectManager { get; set; }

    // Extension points for future modules (nullable FKs — no migration impact)
    public Guid? TaskModuleId { get; set; }
    public Guid? MilestoneModuleId { get; set; }
}
