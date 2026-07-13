using Microsoft.AspNetCore.Identity;

namespace NexAsset.Infrastructure.Identity;

public class ApplicationUser:IdentityUser<Guid>
{
    public Guid? OrganizationId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? DesignationId { get; set; }
    public Guid? EmployeeId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? LoginAtUtc { get; set; } 
}
