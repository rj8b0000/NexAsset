using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public sealed class AssetReturnConfiguration : IEntityTypeConfiguration<AssetReturn>
{
    public void Configure(EntityTypeBuilder<AssetReturn> builder)
    {
        builder.ToTable("AssetReturns");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.InspectionNotes).HasMaxLength(1000);
        builder.Property(x => x.ReturnRemarks).HasMaxLength(1000);
        builder.HasIndex(x => x.AssetId);
        builder.HasOne(x => x.Asset).WithMany().HasForeignKey(x => x.AssetId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}
