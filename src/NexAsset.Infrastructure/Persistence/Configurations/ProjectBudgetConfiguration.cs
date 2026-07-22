using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public sealed class ProjectBudgetConfiguration : IEntityTypeConfiguration<ProjectBudget>
{
    public void Configure(EntityTypeBuilder<ProjectBudget> builder)
    {
        builder.ToTable("ProjectBudgets");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EstimatedBudget)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ApprovedBudget)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ActualCost)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ProcurementCost)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.MaintenanceCost)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.LabourCost)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.MiscellaneousCost)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.UpdatedByUserId)
            .IsRequired();

        // Index for latest-first queries (ProjectId + CreatedAtUtc descending)
        builder.HasIndex(x => new { x.ProjectId, x.CreatedAtUtc });

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
