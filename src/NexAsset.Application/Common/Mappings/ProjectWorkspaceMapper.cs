using NexAsset.Application.Features.ProjectWorkspaces;
using NexAsset.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class ProjectWorkspaceMapper
{
    public static partial ProjectCategoryDto ToDto(ProjectCategory category);
    public static partial ProjectDto ToDto(Project project);
    public static partial ProjectMemberDto ToDto(ProjectMember member);
    public static partial ProjectAssetAllocationDto ToDto(ProjectAssetAllocation allocation);
    public static partial ProjectParameterGroupDto ToDto(ProjectParameterGroup group);
    public static partial ProjectParameterDto ToDto(ProjectParameter parameter);
    public static partial ProjectDocumentDto ToDto(ProjectDocument document);
    public static partial ProjectActivityDto ToDto(ProjectActivity activity);
    public static partial ProjectDraftDto ToDto(ProjectDraft draft);
    public static partial ProjectBudgetDto ToDto(ProjectBudget budget);
    public static partial ProjectRiskDto ToDto(ProjectRisk risk);
    public static partial ProjectSettingDto ToDto(ProjectSetting setting);
}
