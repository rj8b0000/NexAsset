using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Designations.Commands.UpdateDesignation;

public sealed record UpdateDesignationCommand(
    Guid Id,
    Guid OrganizationId,
    Guid? DepartmentId,
    string Title,
    string? Description,
    bool IsActive)
    : IRequest<Result<UpdateDesignationResponse>>;
