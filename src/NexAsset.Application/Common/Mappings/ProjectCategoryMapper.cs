using NexAsset.Application.Features.ProjectCategories.Commands.CreateProjectCategory;
using NexAsset.Application.Features.ProjectCategories.Commands.UpdateProjectCategory;
using NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategories;
using NexAsset.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class ProjectCategoryMapper
{
    [MapperIgnoreTarget(nameof(ProjectCategory.Id))]
    [MapperIgnoreTarget(nameof(ProjectCategory.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectCategory.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectCategory.IsDeleted))]
    [MapperIgnoreTarget(nameof(ProjectCategory.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectCategory.Organization))]
    public static partial ProjectCategory ToEntity(CreateProjectCategoryCommand command);

    public static partial ProjectCategoryResponse ToResponse(ProjectCategory category);

    [MapperIgnoreTarget(nameof(ProjectCategory.Id))]
    [MapperIgnoreTarget(nameof(ProjectCategory.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectCategory.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectCategory.IsDeleted))]
    [MapperIgnoreTarget(nameof(ProjectCategory.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(ProjectCategory.Organization))]
    [MapperIgnoreSource(nameof(UpdateProjectCategoryCommand.Id))]
    public static partial void ApplyUpdate(UpdateProjectCategoryCommand command, ProjectCategory category);
}
