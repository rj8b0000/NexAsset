using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public sealed class ProjectRiskConfiguration : IEntityTypeConfiguration<ProjectRisk>
{
    public void Configure(EntityTypeBuilder<ProjectRisk> builder)
    {
        builder.ToTable("ProjectRisks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Category)
            .IsRequired();

        builder.Property(x => x.Probability)
            .IsRequired();

        builder.Property(x => x.Impact)
            .IsRequired();

        builder.Property(x => x.Severity)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.MitigationPlan)
            .HasMaxLength(1000);

        builder.Property(x => x.Remarks)
            .HasMaxLength(500);

        builder.HasIndex(x => new { x.ProjectId, x.Status });

        builder.HasIndex(x => new { x.ProjectId, x.Severity });

        builder.HasIndex(x => x.OwnerEmployeeId);

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.OwnerEmployee)
            .WithMany()
            .HasForeignKey(x => x.OwnerEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
