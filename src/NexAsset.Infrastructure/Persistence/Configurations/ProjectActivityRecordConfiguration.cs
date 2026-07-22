using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public sealed class ProjectActivityRecordConfiguration : IEntityTypeConfiguration<ProjectActivityRecord>
{
    public void Configure(EntityTypeBuilder<ProjectActivityRecord> builder)
    {
        builder.ToTable("ProjectActivityRecords");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ActivityType)
            .IsRequired();

        builder.Property(x => x.Action)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.TargetEntity)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Timestamp)
            .IsRequired();

        builder.Property(x => x.Remarks)
            .HasMaxLength(500);

        // Index for chronological queries (latest first)
        builder.HasIndex(x => new { x.ProjectId, x.Timestamp });

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
