using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Configurations;

public sealed class ProjectCategoryConfiguration : IEntityTypeConfiguration<ProjectCategory>
{
    public void Configure(EntityTypeBuilder<ProjectCategory> builder)
    {
        builder.ToTable("ProjectCategories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.HasIndex(x => new { x.OrganizationId, x.Name }).IsUnique();
        builder.HasOne(x => x.Organization).WithMany().HasForeignKey(x => x.OrganizationId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ProjectName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.Notes).HasMaxLength(4000);
        builder.HasIndex(x => new { x.OrganizationId, x.ProjectName });
        builder.HasOne(x => x.Organization).WithMany().HasForeignKey(x => x.OrganizationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Client).WithMany().HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Department).WithMany().HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProjectManager).WithMany().HasForeignKey(x => x.ProjectManagerId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.ToTable("ProjectMembers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.RoleInProject).HasMaxLength(100).IsRequired();
        builder.Property(x => x.AllocationPercentage).HasPrecision(5, 2);
        builder.Property(x => x.Remarks).HasMaxLength(1000);
        builder.HasIndex(x => new { x.ProjectId, x.EmployeeId });
        builder.HasOne(x => x.Project).WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProjectAssetAllocationConfiguration : IEntityTypeConfiguration<ProjectAssetAllocation>
{
    public void Configure(EntityTypeBuilder<ProjectAssetAllocation> builder)
    {
        builder.ToTable("ProjectAssetAllocations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Remarks).HasMaxLength(1000);
        builder.HasIndex(x => new { x.ProjectId, x.AssetId });
        builder.HasOne(x => x.Project).WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Asset).WithMany().HasForeignKey(x => x.AssetId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProjectParameterGroupConfiguration : IEntityTypeConfiguration<ProjectParameterGroup>
{
    public void Configure(EntityTypeBuilder<ProjectParameterGroup> builder)
    {
        builder.ToTable("ProjectParameterGroups");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.GroupName).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => new { x.ProjectId, x.DisplayOrder });
        builder.HasOne(x => x.Project).WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProjectParameterConfiguration : IEntityTypeConfiguration<ProjectParameter>
{
    public void Configure(EntityTypeBuilder<ProjectParameter> builder)
    {
        builder.ToTable("ProjectParameters");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ParameterName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Value).HasMaxLength(2000);
        builder.Property(x => x.Unit).HasMaxLength(50);
        builder.HasIndex(x => new { x.ProjectId, x.GroupId, x.DisplayOrder });
        builder.HasOne(x => x.Project).WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Group).WithMany().HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProjectDocumentConfiguration : IEntityTypeConfiguration<ProjectDocument>
{
    public void Configure(EntityTypeBuilder<ProjectDocument> builder)
    {
        builder.ToTable("ProjectDocuments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Category).HasMaxLength(100).IsRequired();
        builder.Property(x => x.DocumentName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.FilePath).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.Remarks).HasMaxLength(1000);
        builder.HasIndex(x => new { x.ProjectId, x.Category });
        builder.HasOne(x => x.Project).WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.UploadedByEmployee).WithMany().HasForeignKey(x => x.UploadedBy).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProjectActivityConfiguration : IEntityTypeConfiguration<ProjectActivity>
{
    public void Configure(EntityTypeBuilder<ProjectActivity> builder)
    {
        builder.ToTable("ProjectActivities");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ActivityType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(1000).IsRequired();
        builder.HasIndex(x => new { x.ProjectId, x.OccurredAtUtc });
        builder.HasOne(x => x.Project).WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ActorEmployee).WithMany().HasForeignKey(x => x.ActorEmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProjectDraftConfiguration : IEntityTypeConfiguration<ProjectDraft>
{
    public void Configure(EntityTypeBuilder<ProjectDraft> builder)
    {
        builder.ToTable("ProjectDrafts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DraftName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.DraftState).HasMaxLength(4000);
        builder.HasIndex(x => new { x.OrganizationId, x.OwnerEmployeeId, x.IsSubmitted });
        builder.HasOne(x => x.Organization).WithMany().HasForeignKey(x => x.OrganizationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Project).WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.OwnerEmployee).WithMany().HasForeignKey(x => x.OwnerEmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProjectBudgetConfiguration : IEntityTypeConfiguration<ProjectBudget>
{
    public void Configure(EntityTypeBuilder<ProjectBudget> builder)
    {
        builder.ToTable("ProjectBudgets");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.EstimatedBudget).HasPrecision(18, 2);
        builder.Property(x => x.ActualCost).HasPrecision(18, 2);
        builder.Property(x => x.ProcurementCost).HasPrecision(18, 2);
        builder.Property(x => x.MaintenanceCost).HasPrecision(18, 2);
        builder.Property(x => x.LabourCost).HasPrecision(18, 2);
        builder.Property(x => x.MiscellaneousCost).HasPrecision(18, 2);
        builder.HasIndex(x => x.ProjectId).IsUnique(); // One budget per project
        builder.HasOne(x => x.Project).WithOne().HasForeignKey<ProjectBudget>(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProjectRiskConfiguration : IEntityTypeConfiguration<ProjectRisk>
{
    public void Configure(EntityTypeBuilder<ProjectRisk> builder)
    {
        builder.ToTable("ProjectRisks");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.Probability).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Impact).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Severity).HasMaxLength(50).IsRequired();
        builder.Property(x => x.MitigationPlan).HasMaxLength(2000);
        builder.Property(x => x.Status).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.ProjectId);
        builder.HasOne(x => x.Project).WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.OwnerEmployee).WithMany().HasForeignKey(x => x.OwnerEmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProjectSettingConfiguration : IEntityTypeConfiguration<ProjectSetting>
{
    public void Configure(EntityTypeBuilder<ProjectSetting> builder)
    {
        builder.ToTable("ProjectSettings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Key).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Value).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.HasIndex(x => new { x.ProjectId, x.Key }).IsUnique();
        builder.HasOne(x => x.Project).WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}
