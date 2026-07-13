using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Branches.Queries.GetBranch;

public sealed record GetBranchQuery(Guid Id)
    : IRequest<Result<GetBranchResponse>>;
