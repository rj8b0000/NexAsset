using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Branches.Commands.UpdateBranch;

public sealed record UpdateBranchCommand(
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
    bool IsActive)
    : IRequest<Result<UpdateBranchResponse>>;
