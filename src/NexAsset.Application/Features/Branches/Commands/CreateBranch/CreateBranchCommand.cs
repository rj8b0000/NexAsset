using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Branches.Commands.CreateBranch;

public sealed record CreateBranchCommand(
    Guid OrganizationId,
    string Code,
    string Name,
    string? Email,
    string? Phone,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode)
    : IRequest<Result<CreateBranchResponse>>;
