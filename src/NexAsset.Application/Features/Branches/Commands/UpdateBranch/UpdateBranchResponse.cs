namespace NexAsset.Application.Features.Branches.Commands.UpdateBranch;

public sealed record UpdateBranchResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive);
