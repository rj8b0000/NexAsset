namespace NexAsset.Application.Features.Branches.Queries.GetBranches;

public sealed record BranchListItemResponse(
    Guid Id,
    Guid OrganizationId,
    string Code,
    string Name,
    string? Email,
    bool IsActive);
