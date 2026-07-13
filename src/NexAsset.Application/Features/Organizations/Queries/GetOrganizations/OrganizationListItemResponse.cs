namespace NexAsset.Application.Features.Organizations.Queries.GetOrganizations;

public sealed record OrganizationListItemResponse(
    Guid Id,
    string Code,
    string Name,
    string Email,
    bool IsActive);