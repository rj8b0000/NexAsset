using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public class DraftSessionConfiguration : IEntityTypeConfiguration<DraftSession>
{
    public void Configure(EntityTypeBuilder<DraftSession> builder)
    {
        builder.ToTable("DraftSessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WizardStateJson)
            .IsRequired();

        builder.HasIndex(x => new { x.UserId, x.OrganizationId });
    }
}
