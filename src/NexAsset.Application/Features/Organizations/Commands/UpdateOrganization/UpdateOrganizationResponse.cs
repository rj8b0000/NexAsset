namespace NexAsset.Application.Features.Organizations.Commands.UpdateOrganization;

public sealed record UpdateOrganizationResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive);
