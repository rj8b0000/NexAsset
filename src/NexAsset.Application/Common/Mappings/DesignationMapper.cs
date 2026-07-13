using NexAsset.Application.Features.Designations.Commands.CreateDesignation;
using NexAsset.Application.Features.Designations.Commands.UpdateDesignation;
using NexAsset.Application.Features.Designations.Queries.GetDesignation;
using NexAsset.Application.Features.Designations.Queries.GetDesignations;
using NexAsset.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class DesignationMapper
{
    [MapperIgnoreTarget(nameof(Designation.Id))]
    [MapperIgnoreTarget(nameof(Designation.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Designation.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Designation.IsDeleted))]
    [MapperIgnoreTarget(nameof(Designation.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Designation.Organization))]
    [MapperIgnoreTarget(nameof(Designation.Department))]
    public static partial Designation ToEntity(
        CreateDesignationCommand command);

    public static partial CreateDesignationResponse ToResponse(
        Designation designation);

    public static partial GetDesignationResponse ToGetResponse(
        Designation designation);

    public static partial DesignationListItemResponse ToListItemResponse(
        Designation designation);

    [MapperIgnoreTarget(nameof(Designation.Id))]
    [MapperIgnoreTarget(nameof(Designation.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Designation.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Designation.IsDeleted))]
    [MapperIgnoreTarget(nameof(Designation.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Designation.Organization))]
    [MapperIgnoreTarget(nameof(Designation.Department))]
    [MapperIgnoreSource(nameof(UpdateDesignationCommand.Id))]
    public static partial void ApplyUpdate(
        UpdateDesignationCommand command,
        Designation designation);
}
