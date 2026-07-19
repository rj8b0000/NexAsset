using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Identity;

namespace NexAsset.Infrastructure.Persistence;

public class ApplicationDbContext:IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private readonly ITenantContext? _tenant;

    // Null when there is no organization boundary to apply — SuperAdmin, startup seeding, or
    // design-time tooling. Read per query, so one context instance follows the current request.
    private Guid? TenantOrganizationId => _tenant?.FilterOrganizationId;

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
        // An organization is visible only to its own members.
        modelBuilder.Entity<Organization>()
            .HasQueryFilter(x => TenantOrganizationId == null || x.Id == TenantOrganizationId);

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
