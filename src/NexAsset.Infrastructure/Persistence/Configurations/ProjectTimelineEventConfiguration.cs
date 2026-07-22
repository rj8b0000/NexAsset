using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public sealed class ProjectTimelineEventConfiguration : IEntityTypeConfiguration<ProjectTimelineEvent>
{
    public void Configure(EntityTypeBuilder<ProjectTimelineEvent> builder)
    {
        builder.ToTable("ProjectTimelineEvents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventType)
            .IsRequired();

        builder.Property(x => x.EntityType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Timestamp)
            .IsRequired();

        builder.Property(x => x.IconType)
            .HasMaxLength(50)
            .IsRequired();

        // Index for chronological queries (latest first)
        builder.HasIndex(x => new { x.ProjectId, x.Timestamp });

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
