using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public sealed class DesignationPermissionConfiguration : IEntityTypeConfiguration<DesignationPermission>
{
    public void Configure(EntityTypeBuilder<DesignationPermission> builder)
    {
        builder.ToTable("DesignationPermissions");

        builder.HasKey(x => new { x.DesignationId, x.PermissionId });

        builder.HasOne(x => x.Permission)
            .WithMany()
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
