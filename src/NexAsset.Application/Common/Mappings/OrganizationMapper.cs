using Riok.Mapperly.Abstractions;
using NexAsset.Domain.Entities;
using NexAsset.Application.Features.Organizations.Commands.CreateOrganization;
using NexAsset.Application.Features.Organizations.Commands.UpdateOrganization;
using NexAsset.Application.Features.Organizations.Queries.GetOrganization;
using NexAsset.Application.Features.Organizations.Queries.GetOrganizations;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class OrganizationMapper
{
    public static partial Organization ToEntity(
        CreateOrganizationCommand command);

    public static partial CreateOrganizationResponse ToResponse(
        Organization organization);

    public static partial GetOrganizationResponse ToGetResponse(
        Organization organization);

    public static partial OrganizationListItemResponse ToListItemResponse(
        Organization organization);

    [MapperIgnoreTarget(nameof(Organization.Id))]
    [MapperIgnoreTarget(nameof(Organization.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Organization.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Organization.IsDeleted))]
    [MapperIgnoreTarget(nameof(Organization.DeletedAtUtc))]
    [MapperIgnoreSource(nameof(UpdateOrganizationCommand.Id))]
    public static partial void ApplyUpdate(
        UpdateOrganizationCommand command,
        Organization organization);
}
