using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public sealed class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("Assets");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AssetCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.AssetName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.SerialNumber).HasMaxLength(100);
        builder.Property(x => x.Barcode).HasMaxLength(100);
        builder.Property(x => x.QrCode).HasMaxLength(500);
        builder.Property(x => x.Brand).HasMaxLength(100);
        builder.Property(x => x.Model).HasMaxLength(100);
        builder.Property(x => x.Vendor).HasMaxLength(200);
        builder.Property(x => x.PurchaseCost).HasPrecision(18, 2);
        builder.Property(x => x.CurrentValue).HasPrecision(18, 2);
        builder.Property(x => x.Location).HasMaxLength(200);
        builder.Property(x => x.Remarks).HasMaxLength(1000);
        builder.HasIndex(x => new { x.OrganizationId, x.AssetCode }).IsUnique();
        builder.HasIndex(x => x.SerialNumber).IsUnique();
        builder.HasOne(x => x.Organization).WithMany().HasForeignKey(x => x.OrganizationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Department).WithMany().HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CurrentEmployee).WithMany().HasForeignKey(x => x.CurrentEmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}
