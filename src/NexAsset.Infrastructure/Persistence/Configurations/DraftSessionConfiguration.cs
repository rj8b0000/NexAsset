using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public sealed class DraftSessionConfiguration : IEntityTypeConfiguration<DraftSession>
{
    public void Configure(EntityTypeBuilder<DraftSession> builder)
    {
        builder.ToTable("DraftSessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WizardStateJson)
            .IsRequired();

        builder.Property(x => x.CurrentStep)
            .IsRequired();

        builder.Property(x => x.LastSavedAtUtc)
            .IsRequired();

        builder.HasIndex(x => new { x.UserId, x.OrganizationId })
            .HasFilter("\"IsDeleted\" = false");
    }
}
