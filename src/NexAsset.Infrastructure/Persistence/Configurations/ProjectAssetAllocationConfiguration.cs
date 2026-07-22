using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public sealed class ProjectAssetAllocationConfiguration : IEntityTypeConfiguration<ProjectAssetAllocation>
{
    public void Configure(EntityTypeBuilder<ProjectAssetAllocation> builder)
    {
        builder.ToTable("ProjectAssetAllocations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AllocationDate)
            .IsRequired();

        builder.Property(x => x.AllocatedQuantity)
            .IsRequired();

        builder.Property(x => x.ReturnedQuantity)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.Remarks)
            .HasMaxLength(500);

        builder.HasIndex(x => new { x.ProjectId, x.Status });

        builder.HasIndex(x => x.AssetId);

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Asset)
            .WithMany()
            .HasForeignKey(x => x.AssetId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
