namespace NexAsset.Application.Features.Branches.Commands.UpdateBranch;

public sealed record UpdateBranchRequest(
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
