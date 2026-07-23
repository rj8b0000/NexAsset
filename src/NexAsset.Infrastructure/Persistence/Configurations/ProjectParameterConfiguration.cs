using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public class ProjectParameterConfiguration : IEntityTypeConfiguration<ProjectParameter>
{
    public void Configure(EntityTypeBuilder<ProjectParameter> builder)
    {
        builder.ToTable("ProjectParameters");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ParameterName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Unit)
            .HasMaxLength(50);

        builder.HasIndex(x => x.SectionId);

        builder.HasOne(x => x.Section)
            .WithMany(s => s.Parameters)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
