using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public sealed class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable("Vendors");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(50);
        builder.Property(x => x.ContactPerson).HasMaxLength(200);
        builder.Property(x => x.TaxNumber).HasMaxLength(100);
        builder.HasIndex(x => new { x.OrganizationId, x.Code }).IsUnique();
        builder.HasOne(x => x.Organization).WithMany().HasForeignKey(x => x.OrganizationId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class PurchaseRequestConfiguration : IEntityTypeConfiguration<PurchaseRequest>
{
    public void Configure(EntityTypeBuilder<PurchaseRequest> builder)
    {
        builder.ToTable("PurchaseRequests");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.RequestNumber).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.ApprovalRemarks).HasMaxLength(1000);
        builder.Property(x => x.EstimatedAmount).HasPrecision(18, 2);
        builder.HasIndex(x => new { x.OrganizationId, x.RequestNumber }).IsUnique();
        builder.HasOne(x => x.RequestedByEmployee).WithMany().HasForeignKey(x => x.RequestedByEmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrders");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OrderNumber).HasMaxLength(50).IsRequired();
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        builder.Property(x => x.Remarks).HasMaxLength(1000);
        builder.HasIndex(x => new { x.OrganizationId, x.OrderNumber }).IsUnique();
        builder.HasOne(x => x.PurchaseRequest).WithMany().HasForeignKey(x => x.PurchaseRequestId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Vendor).WithMany().HasForeignKey(x => x.VendorId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("InventoryItems");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ItemCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.UnitOfMeasure).HasMaxLength(50).IsRequired();
        builder.Ignore(x => x.AvailableStock);
        builder.HasIndex(x => new { x.OrganizationId, x.ItemCode }).IsUnique();
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ReferenceNumber).HasMaxLength(100);
        builder.Property(x => x.Remarks).HasMaxLength(1000);
        builder.HasIndex(x => x.InventoryItemId);
        builder.HasOne(x => x.InventoryItem).WithMany().HasForeignKey(x => x.InventoryItemId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ConsumableConfiguration : IEntityTypeConfiguration<Consumable>
{
    public void Configure(EntityTypeBuilder<Consumable> builder)
    {
        builder.ToTable("Consumables");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ConsumableCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.HasIndex(x => x.ConsumableCode).IsUnique();
        builder.HasOne(x => x.InventoryItem).WithMany().HasForeignKey(x => x.InventoryItemId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class MaintenanceRecordConfiguration : IEntityTypeConfiguration<MaintenanceRecord>
{
    public void Configure(EntityTypeBuilder<MaintenanceRecord> builder)
    {
        builder.ToTable("MaintenanceRecords");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Remarks).HasMaxLength(1000);
        builder.Property(x => x.Cost).HasPrecision(18, 2);
        builder.HasOne(x => x.Asset).WithMany().HasForeignKey(x => x.AssetId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(50);
        builder.Property(x => x.ContactPerson).HasMaxLength(200);
        builder.HasIndex(x => new { x.OrganizationId, x.Code }).IsUnique();
    }
}

public sealed class ServiceTicketConfiguration : IEntityTypeConfiguration<ServiceTicket>
{
    public void Configure(EntityTypeBuilder<ServiceTicket> builder)
    {
        builder.ToTable("ServiceTickets");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TicketNumber).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Resolution).HasMaxLength(1000);
        builder.Property(x => x.Comments).HasMaxLength(1000);
        builder.HasIndex(x => new { x.OrganizationId, x.TicketNumber }).IsUnique();
        builder.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(1000).IsRequired();
    }
}

public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.EntityName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Action).HasMaxLength(100).IsRequired();
    }
}

public sealed class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
{
    public void Configure(EntityTypeBuilder<SystemSetting> builder)
    {
        builder.ToTable("SystemSettings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Key).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Value).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.HasIndex(x => new { x.OrganizationId, x.Key }).IsUnique();
    }
}
