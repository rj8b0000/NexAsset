namespace NexAsset.Application.Features.Organizations.Queries.GetOrganization;

public sealed record GetOrganizationResponse(
    Guid Id,
    string Code,
    string Name,
    string LegalName,
    string Email,
    string? Phone,
    string? Website,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string Currency,
    string TimeZone,
    bool IsActive);