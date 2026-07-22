using NexAsset.Application.Features.ProjectCategories;
using NexAsset.Application.Features.Projects;
using NexAsset.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class ProjectWorkspaceMapper
{
    [MapperIgnoreTarget(nameof(ProjectCategory.Id))]
    [MapperIgnoreTarget(nameof(ProjectCategory.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectCategory.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectCategory.IsDeleted))]
    [MapperIgnoreTarget(nameof(ProjectCategory.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectCategory.Organization))]
    public static partial ProjectCategory ToEntity(CreateProjectCategoryCommand command);

    [MapperIgnoreTarget(nameof(ProjectCategory.Id))]
    [MapperIgnoreTarget(nameof(ProjectCategory.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectCategory.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectCategory.IsDeleted))]
    [MapperIgnoreTarget(nameof(ProjectCategory.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectCategory.Organization))]
    [MapperIgnoreSource(nameof(UpdateProjectCategoryCommand.Id))]
    public static partial void ApplyUpdate(UpdateProjectCategoryCommand command, ProjectCategory category);

    public static partial ProjectCategoryResponse ToResponse(ProjectCategory category);
    public static partial ProjectCategoryListItemResponse ToListItemResponse(ProjectCategory category);

    [MapperIgnoreTarget(nameof(Project.Id))]
    [MapperIgnoreTarget(nameof(Project.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Project.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Project.IsDeleted))]
    [MapperIgnoreTarget(nameof(Project.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Project.Status))]
    [MapperIgnoreTarget(nameof(Project.TaskModuleId))]
    [MapperIgnoreTarget(nameof(Project.MilestoneModuleId))]
    [MapperIgnoreTarget(nameof(Project.Organization))]
    [MapperIgnoreTarget(nameof(Project.Category))]
    [MapperIgnoreTarget(nameof(Project.Customer))]
    [MapperIgnoreTarget(nameof(Project.Branch))]
    [MapperIgnoreTarget(nameof(Project.Department))]
    [MapperIgnoreTarget(nameof(Project.ProjectManager))]
    public static partial Project ToEntity(CreateProjectCommand command);

    [MapperIgnoreTarget(nameof(Project.Id))]
    [MapperIgnoreTarget(nameof(Project.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Project.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Project.IsDeleted))]
    [MapperIgnoreTarget(nameof(Project.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Project.Status))]
    [MapperIgnoreTarget(nameof(Project.TaskModuleId))]
    [MapperIgnoreTarget(nameof(Project.MilestoneModuleId))]
    [MapperIgnoreTarget(nameof(Project.Organization))]
    [MapperIgnoreTarget(nameof(Project.Category))]
    [MapperIgnoreTarget(nameof(Project.Customer))]
    [MapperIgnoreTarget(nameof(Project.Branch))]
    [MapperIgnoreTarget(nameof(Project.Department))]
    [MapperIgnoreTarget(nameof(Project.ProjectManager))]
    [MapperIgnoreSource(nameof(UpdateProjectCommand.Id))]
    public static partial void ApplyUpdate(UpdateProjectCommand command, Project project);

    public static partial ProjectResponse ToResponse(Project project);
    public static partial ProjectListItemResponse ToListItemResponse(Project project);

    [MapperIgnoreTarget(nameof(DraftSession.Id))]
    [MapperIgnoreTarget(nameof(DraftSession.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(DraftSession.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(DraftSession.IsDeleted))]
    [MapperIgnoreTarget(nameof(DraftSession.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(DraftSession.UserId))]
    [MapperIgnoreTarget(nameof(DraftSession.LastSavedAtUtc))]
    public static partial DraftSession ToEntity(UpsertDraftSessionCommand command);
    public static partial DraftSessionResponse ToResponse(DraftSession session);

    [MapperIgnoreTarget(nameof(ProjectTeamMember.Id))]
    [MapperIgnoreTarget(nameof(ProjectTeamMember.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectTeamMember.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectTeamMember.IsDeleted))]
    [MapperIgnoreTarget(nameof(ProjectTeamMember.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectTeamMember.Status))]
    [MapperIgnoreTarget(nameof(ProjectTeamMember.SnapshotBranchId))]
    [MapperIgnoreTarget(nameof(ProjectTeamMember.SnapshotDepartmentId))]
    [MapperIgnoreTarget(nameof(ProjectTeamMember.Project))]
    [MapperIgnoreTarget(nameof(ProjectTeamMember.Employee))]
    public static partial ProjectTeamMember ToEntity(AddTeamMemberCommand command);
    public static partial TeamMemberResponse ToResponse(ProjectTeamMember member);

    [MapperIgnoreTarget(nameof(ProjectAssetAllocation.Id))]
    [MapperIgnoreTarget(nameof(ProjectAssetAllocation.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectAssetAllocation.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectAssetAllocation.IsDeleted))]
    [MapperIgnoreTarget(nameof(ProjectAssetAllocation.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectAssetAllocation.ReturnDate))]
    [MapperIgnoreTarget(nameof(ProjectAssetAllocation.ReturnedQuantity))]
    [MapperIgnoreTarget(nameof(ProjectAssetAllocation.Status))]
    [MapperIgnoreTarget(nameof(ProjectAssetAllocation.Project))]
    [MapperIgnoreTarget(nameof(ProjectAssetAllocation.Asset))]
    public static partial ProjectAssetAllocation ToEntity(AllocateAssetCommand command);
    public static partial AssetAllocationResponse ToResponse(ProjectAssetAllocation allocation);

    [MapperIgnoreTarget(nameof(ProjectDocument.Id))]
    [MapperIgnoreTarget(nameof(ProjectDocument.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectDocument.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectDocument.IsDeleted))]
    [MapperIgnoreTarget(nameof(ProjectDocument.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectDocument.FileReference))]
    [MapperIgnoreTarget(nameof(ProjectDocument.UploadedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectDocument.Version))]
    [MapperIgnoreTarget(nameof(ProjectDocument.UploadedByEmployeeId))]
    [MapperIgnoreTarget(nameof(ProjectDocument.Project))]
    [MapperIgnoreTarget(nameof(ProjectDocument.UploadedByEmployee))]
    public static partial ProjectDocument ToEntity(UploadDocumentCommand command);
    public static partial DocumentResponse ToResponse(ProjectDocument document);

    [MapperIgnoreTarget(nameof(ProjectParameterSection.Id))]
    [MapperIgnoreTarget(nameof(ProjectParameterSection.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectParameterSection.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectParameterSection.IsDeleted))]
    [MapperIgnoreTarget(nameof(ProjectParameterSection.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectParameterSection.Project))]
    [MapperIgnoreTarget(nameof(ProjectParameterSection.Parameters))]
    public static partial ProjectParameterSection ToEntity(CreateParameterSectionCommand command);
    public static partial ParameterSectionResponse ToResponse(ProjectParameterSection section);

    [MapperIgnoreTarget(nameof(ProjectParameter.Id))]
    [MapperIgnoreTarget(nameof(ProjectParameter.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectParameter.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectParameter.IsDeleted))]
    [MapperIgnoreTarget(nameof(ProjectParameter.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectParameter.Section))]
    [MapperIgnoreSource(nameof(AddParameterCommand.ProjectId))]
    public static partial ProjectParameter ToEntity(AddParameterCommand command);
    public static partial ParameterResponse ToResponse(ProjectParameter parameter);

    [MapperIgnoreTarget(nameof(ProjectBudget.Id))]
    [MapperIgnoreTarget(nameof(ProjectBudget.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectBudget.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectBudget.IsDeleted))]
    [MapperIgnoreTarget(nameof(ProjectBudget.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectBudget.UpdatedByUserId))]
    [MapperIgnoreTarget(nameof(ProjectBudget.FinanceInvoiceId))]
    [MapperIgnoreTarget(nameof(ProjectBudget.Project))]
    public static partial ProjectBudget ToEntity(UpdateBudgetCommand command);
    public static partial BudgetResponse ToResponse(ProjectBudget budget);

    [MapperIgnoreTarget(nameof(ProjectRisk.Id))]
    [MapperIgnoreTarget(nameof(ProjectRisk.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectRisk.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectRisk.IsDeleted))]
    [MapperIgnoreTarget(nameof(ProjectRisk.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectRisk.Severity))]
    [MapperIgnoreTarget(nameof(ProjectRisk.Status))]
    [MapperIgnoreTarget(nameof(ProjectRisk.ClosedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectRisk.Project))]
    [MapperIgnoreTarget(nameof(ProjectRisk.OwnerEmployee))]
    public static partial ProjectRisk ToEntity(CreateRiskCommand command);
    public static partial RiskResponse ToResponse(ProjectRisk risk);

    public static partial TimelineEventResponse ToResponse(ProjectTimelineEvent timelineEvent);
    public static partial ActivityRecordResponse ToResponse(ProjectActivityRecord activity);
    public static partial SavedFilterResponse ToResponse(SavedFilter filter);

    [MapperIgnoreTarget(nameof(SavedFilter.Id))]
    [MapperIgnoreTarget(nameof(SavedFilter.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(SavedFilter.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(SavedFilter.IsDeleted))]
    [MapperIgnoreTarget(nameof(SavedFilter.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(SavedFilter.UserId))]
    [MapperIgnoreTarget(nameof(SavedFilter.Organization))]
    public static partial SavedFilter ToEntity(SaveFilterCommand command);
}
