namespace NexAsset.Application.Features.Branches.Queries.GetBranch;

public sealed record GetBranchResponse(
    Guid Id,
    Guid OrganizationId,
    string Code,
    string Name,
    string? Email,
    string? Phone,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    bool IsActive);
