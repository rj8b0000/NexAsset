using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Identity;

namespace NexAsset.Infrastructure.Persistence;

public class ApplicationDbContext:IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private readonly ITenantContext? _tenant;

    // Null when there is no organization boundary to apply — an unscoped SuperAdmin, startup
    // seeding, or design-time tooling. Read per query, so one context instance follows the
    // current request.
    private Guid? TenantOrganizationId => _tenant?.FilterOrganizationId;

    // Scope for the Organization entity itself; null for any SuperAdmin so the switcher and
    // Organizations page always list every organization.
    private Guid? OrganizationScopeId => _tenant?.OrganizationFilterId;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ITenantContext? tenant = null) : base(options)
    {
        _tenant = tenant;
    }

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Designation> Designations => Set<Designation>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<DesignationPermission> DesignationPermissions => Set<DesignationPermission>();
    public DbSet<AssetCategory> AssetCategories => Set<AssetCategory>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<AssetAssignment> AssetAssignments => Set<AssetAssignment>();
    public DbSet<AssetTransfer> AssetTransfers => Set<AssetTransfer>();
    public DbSet<AssetReturn> AssetReturns => Set<AssetReturn>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<PurchaseRequest> PurchaseRequests => Set<PurchaseRequest>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<Consumable> Consumables => Set<Consumable>();
    public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<ServiceTicket> ServiceTickets => Set<ServiceTicket>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
    public DbSet<ProjectCategory> ProjectCategories => Set<ProjectCategory>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<DraftSession> DraftSessions => Set<DraftSession>();
    public DbSet<ProjectParameterSection> ProjectParameterSections => Set<ProjectParameterSection>();
    public DbSet<ProjectParameter> ProjectParameters => Set<ProjectParameter>();
    public DbSet<ProjectParameterValue> ProjectParameterValues => Set<ProjectParameterValue>();
    public DbSet<ProjectTeamMember> ProjectTeamMembers => Set<ProjectTeamMember>();
    public DbSet<ProjectAssetAllocation> ProjectAssetAllocations => Set<ProjectAssetAllocation>();
    public DbSet<ProjectDocument> ProjectDocuments => Set<ProjectDocument>();
    public DbSet<ProjectBudget> ProjectBudgets => Set<ProjectBudget>();
    public DbSet<ProjectRisk> ProjectRisks => Set<ProjectRisk>();
    public DbSet<ProjectTimelineEvent> ProjectTimelineEvents => Set<ProjectTimelineEvent>();
    public DbSet<ProjectActivityRecord> ProjectActivityRecords => Set<ProjectActivityRecord>();
    public DbSet<SavedFilter> SavedFilters => Set<SavedFilter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        ApplyOrganizationBoundary(modelBuilder);
    }

    /// <summary>
    /// Restricts every organization-scoped entity to the caller's organization. Enforcing this in
    /// the model means no query handler can forget it: a non-SuperAdmin simply cannot read another
    /// organization's rows, and passing a foreign id just yields "not found".
    /// </summary>
    private void ApplyOrganizationBoundary(ModelBuilder modelBuilder)
    {
        // An organization is visible only to its own members — but a SuperAdmin always sees all,
        // even while their workspace is focused on one organization (OrganizationScopeId stays null).
        modelBuilder.Entity<Organization>()
            .HasQueryFilter(x => OrganizationScopeId == null || x.Id == OrganizationScopeId);

        modelBuilder.Entity<Branch>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<Department>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<Designation>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<Employee>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<AssetCategory>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<Asset>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<AssetAssignment>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<Vendor>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<Customer>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<PurchaseRequest>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<PurchaseOrder>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<InventoryItem>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<ServiceTicket>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);

        // System settings are either organization-specific or global (null = visible to everyone).
        modelBuilder.Entity<SystemSetting>()
            .HasQueryFilter(x => TenantOrganizationId == null
                                 || x.OrganizationId == null
                                 || x.OrganizationId == TenantOrganizationId);

        modelBuilder.Entity<ProjectCategory>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);

        modelBuilder.Entity<Project>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);

        // Project child entities carry no direct OrganizationId; they inherit via Project.
        modelBuilder.Entity<DraftSession>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);

        modelBuilder.Entity<SavedFilter>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.OrganizationId == TenantOrganizationId);

        modelBuilder.Entity<ProjectParameterSection>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Project.OrganizationId == TenantOrganizationId);

        modelBuilder.Entity<ProjectParameter>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Section.Project.OrganizationId == TenantOrganizationId);

        modelBuilder.Entity<ProjectParameterValue>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Project.OrganizationId == TenantOrganizationId);

        modelBuilder.Entity<ProjectTeamMember>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Project.OrganizationId == TenantOrganizationId);

        modelBuilder.Entity<ProjectAssetAllocation>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Project.OrganizationId == TenantOrganizationId);

        modelBuilder.Entity<ProjectDocument>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Project.OrganizationId == TenantOrganizationId);

        modelBuilder.Entity<ProjectBudget>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Project.OrganizationId == TenantOrganizationId);

        modelBuilder.Entity<ProjectRisk>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Project.OrganizationId == TenantOrganizationId);

        modelBuilder.Entity<ProjectTimelineEvent>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Project.OrganizationId == TenantOrganizationId);

        modelBuilder.Entity<ProjectActivityRecord>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Project.OrganizationId == TenantOrganizationId);

        // These carry no OrganizationId of their own; they inherit their parent's boundary.
        modelBuilder.Entity<MaintenanceRecord>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Asset.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<AssetTransfer>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Asset.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<AssetReturn>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Asset.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<Consumable>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.InventoryItem.OrganizationId == TenantOrganizationId);
        modelBuilder.Entity<StockMovement>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.InventoryItem.OrganizationId == TenantOrganizationId);
    }
}
