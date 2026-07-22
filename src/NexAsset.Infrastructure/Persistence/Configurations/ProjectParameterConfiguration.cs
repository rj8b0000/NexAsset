using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public sealed class ProjectParameterConfiguration : IEntityTypeConfiguration<ProjectParameter>
{
    public void Configure(EntityTypeBuilder<ProjectParameter> builder)
    {
        builder.ToTable("ProjectParameters");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ParameterName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.InputType)
            .IsRequired();

        builder.Property(x => x.Unit)
            .HasMaxLength(50);

        builder.Property(x => x.IsRequired)
            .IsRequired();

        builder.Property(x => x.DisplayOrder)
            .IsRequired();

        builder.HasIndex(x => x.SectionId);

        builder.HasOne(x => x.Section)
            .WithMany(x => x.Parameters)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
