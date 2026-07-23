using NexAsset.Application.Features.Projects.Commands.CreateProject;
using NexAsset.Application.Features.Projects.Commands.UpdateProject;
using NexAsset.Application.Features.Projects.Queries.GetProjects;
using NexAsset.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class ProjectMapper
{
    [MapperIgnoreTarget(nameof(Project.Id))]
    [MapperIgnoreTarget(nameof(Project.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Project.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Project.IsDeleted))]
    [MapperIgnoreTarget(nameof(Project.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Project.Organization))]
    [MapperIgnoreTarget(nameof(Project.Category))]
    [MapperIgnoreTarget(nameof(Project.Customer))]
    [MapperIgnoreTarget(nameof(Project.Branch))]
    [MapperIgnoreTarget(nameof(Project.Department))]
    [MapperIgnoreTarget(nameof(Project.ProjectManager))]
    [MapperIgnoreTarget(nameof(Project.TaskModuleId))]
    [MapperIgnoreTarget(nameof(Project.MilestoneModuleId))]
    public static partial Project ToEntity(CreateProjectCommand command);

    [MapProperty(nameof(Project.Category.Name), nameof(ProjectResponse.CategoryName))]
    [MapProperty(nameof(Project.Customer.Name), nameof(ProjectResponse.CustomerName))]
    [MapProperty(nameof(Project.Branch.Name), nameof(ProjectResponse.BranchName))]
    [MapProperty(nameof(Project.Department.Name), nameof(ProjectResponse.DepartmentName))]
    [MapProperty(nameof(Project.ProjectManager.FirstName), nameof(ProjectResponse.ProjectManagerName))]
    public static partial ProjectResponse ToResponse(Project project);

    [MapProperty(nameof(Project.Category.Name), nameof(ProjectListItemResponse.CategoryName))]
    [MapProperty(nameof(Project.ProjectManager.FirstName), nameof(ProjectListItemResponse.ProjectManagerName))]
    public static partial ProjectListItemResponse ToListItemResponse(Project project);

    [MapperIgnoreTarget(nameof(Project.Id))]
    [MapperIgnoreTarget(nameof(Project.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Project.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Project.IsDeleted))]
    [MapperIgnoreTarget(nameof(Project.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Project.Organization))]
    [MapperIgnoreTarget(nameof(Project.Category))]
    [MapperIgnoreTarget(nameof(Project.Customer))]
    [MapperIgnoreTarget(nameof(Project.Branch))]
    [MapperIgnoreTarget(nameof(Project.Department))]
    [MapperIgnoreTarget(nameof(Project.ProjectManager))]
    [MapperIgnoreTarget(nameof(Project.TaskModuleId))]
    [MapperIgnoreTarget(nameof(Project.MilestoneModuleId))]
    [MapperIgnoreSource(nameof(UpdateProjectCommand.Id))]
    public static partial void ApplyUpdate(UpdateProjectCommand command, Project project);
}
