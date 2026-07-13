using NexAsset.Application.Features.Employees.Commands.CreateEmployee;
using NexAsset.Application.Features.Employees.Commands.UpdateEmployee;
using NexAsset.Application.Features.Employees.Queries.GetEmployee;
using NexAsset.Application.Features.Employees.Queries.GetEmployees;
using NexAsset.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class EmployeeMapper
{
    [MapperIgnoreTarget(nameof(Employee.Id))]
    [MapperIgnoreTarget(nameof(Employee.IdentityUserId))]
    [MapperIgnoreTarget(nameof(Employee.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Employee.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Employee.IsDeleted))]
    [MapperIgnoreTarget(nameof(Employee.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Employee.Organization))]
    [MapperIgnoreTarget(nameof(Employee.Branch))]
    [MapperIgnoreTarget(nameof(Employee.Department))]
    [MapperIgnoreTarget(nameof(Employee.Designation))]
    [MapperIgnoreTarget(nameof(Employee.ReportingManager))]
    [MapperIgnoreSource(nameof(CreateEmployeeCommand.Password))]
    [MapperIgnoreSource(nameof(CreateEmployeeCommand.Roles))]
    public static partial Employee ToEntity(
        CreateEmployeeCommand command);

    public static partial CreateEmployeeResponse ToResponse(
        Employee employee);

    public static partial GetEmployeeResponse ToGetResponse(
        Employee employee);

    public static partial EmployeeListItemResponse ToListItemResponse(
        Employee employee);

    [MapperIgnoreTarget(nameof(Employee.Id))]
    [MapperIgnoreTarget(nameof(Employee.IdentityUserId))]
    [MapperIgnoreTarget(nameof(Employee.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Employee.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Employee.IsDeleted))]
    [MapperIgnoreTarget(nameof(Employee.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Employee.Organization))]
    [MapperIgnoreTarget(nameof(Employee.Branch))]
    [MapperIgnoreTarget(nameof(Employee.Department))]
    [MapperIgnoreTarget(nameof(Employee.Designation))]
    [MapperIgnoreTarget(nameof(Employee.ReportingManager))]
    [MapperIgnoreSource(nameof(UpdateEmployeeCommand.Id))]
    public static partial void ApplyUpdate(
        UpdateEmployeeCommand command,
        Employee employee);
}
