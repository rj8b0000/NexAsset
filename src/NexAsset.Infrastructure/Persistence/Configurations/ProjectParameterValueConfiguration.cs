using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public class ProjectParameterValueConfiguration : IEntityTypeConfiguration<ProjectParameterValue>
{
    public void Configure(EntityTypeBuilder<ProjectParameterValue> builder)
    {
        builder.ToTable("ProjectParameterValues");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Value)
            .HasMaxLength(2000);

        builder.HasIndex(x => new { x.ProjectId, x.ParameterId })
            .IsUnique();

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Parameter)
            .WithMany()
            .HasForeignKey(x => x.ParameterId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
