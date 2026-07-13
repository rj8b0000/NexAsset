using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Branches.Commands.DeleteBranch;

public sealed record DeleteBranchCommand(Guid Id)
    : IRequest<Result>;
