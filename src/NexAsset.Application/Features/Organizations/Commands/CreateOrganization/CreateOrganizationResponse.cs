namespace NexAsset.Application.Features.Organizations.Commands.CreateOrganization;

public sealed record CreateOrganizationResponse(
    Guid Id,
    string Name);