using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public sealed class AssetTransferConfiguration : IEntityTypeConfiguration<AssetTransfer>
{
    public void Configure(EntityTypeBuilder<AssetTransfer> builder)
    {
        builder.ToTable("AssetTransfers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Remarks).HasMaxLength(1000);
        builder.HasIndex(x => x.AssetId);
        builder.HasOne(x => x.Asset).WithMany().HasForeignKey(x => x.AssetId).OnDelete(DeleteBehavior.Restrict);
    }
}
