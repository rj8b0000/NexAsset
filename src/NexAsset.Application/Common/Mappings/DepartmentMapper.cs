using NexAsset.Application.Features.Departments.Commands.CreateDepartment;
using NexAsset.Application.Features.Departments.Commands.UpdateDepartment;
using NexAsset.Application.Features.Departments.Queries.GetDepartment;
using NexAsset.Application.Features.Departments.Queries.GetDepartments;
using NexAsset.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class DepartmentMapper
{
    [MapperIgnoreTarget(nameof(Department.Id))]
    [MapperIgnoreTarget(nameof(Department.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Department.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Department.IsDeleted))]
    [MapperIgnoreTarget(nameof(Department.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Department.Organization))]
    public static partial Department ToEntity(
        CreateDepartmentCommand command);

    public static partial CreateDepartmentResponse ToResponse(
        Department department);

    public static partial GetDepartmentResponse ToGetResponse(
        Department department);

    public static partial DepartmentListItemResponse ToListItemResponse(
        Department department);

    [MapperIgnoreTarget(nameof(Department.Id))]
    [MapperIgnoreTarget(nameof(Department.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Department.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Department.IsDeleted))]
    [MapperIgnoreTarget(nameof(Department.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Department.Organization))]
    [MapperIgnoreSource(nameof(UpdateDepartmentCommand.Id))]
    public static partial void ApplyUpdate(
        UpdateDepartmentCommand command,
        Department department);
}
