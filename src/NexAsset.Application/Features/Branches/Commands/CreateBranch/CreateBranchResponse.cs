namespace NexAsset.Application.Features.Branches.Commands.CreateBranch;

public sealed record CreateBranchResponse(
    Guid Id,
    string Code,
    string Name);
