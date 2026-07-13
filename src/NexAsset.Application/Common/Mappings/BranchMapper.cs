using NexAsset.Application.Features.Branches.Commands.CreateBranch;
using NexAsset.Application.Features.Branches.Commands.UpdateBranch;
using NexAsset.Application.Features.Branches.Queries.GetBranch;
using NexAsset.Application.Features.Branches.Queries.GetBranches;
using NexAsset.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class BranchMapper
{
    [MapperIgnoreTarget(nameof(Branch.Id))]
    [MapperIgnoreTarget(nameof(Branch.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Branch.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Branch.IsDeleted))]
    [MapperIgnoreTarget(nameof(Branch.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Branch.Organization))]
    public static partial Branch ToEntity(
        CreateBranchCommand command);

    public static partial CreateBranchResponse ToResponse(
        Branch branch);

    public static partial GetBranchResponse ToGetResponse(
        Branch branch);

    public static partial BranchListItemResponse ToListItemResponse(
        Branch branch);

    [MapperIgnoreTarget(nameof(Branch.Id))]
    [MapperIgnoreTarget(nameof(Branch.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Branch.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Branch.IsDeleted))]
    [MapperIgnoreTarget(nameof(Branch.DeletedAtUtc))]
    [MapperIgnoreTarget(nameof(Branch.Organization))]
    [MapperIgnoreSource(nameof(UpdateBranchCommand.Id))]
    public static partial void ApplyUpdate(
        UpdateBranchCommand command,
        Branch branch);
}
