using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Designations.Commands.CreateDesignation;

public sealed record CreateDesignationCommand(
    Guid OrganizationId,
    Guid? DepartmentId,
    string Title,
    string? Description)
    : IRequest<Result<CreateDesignationResponse>>;
