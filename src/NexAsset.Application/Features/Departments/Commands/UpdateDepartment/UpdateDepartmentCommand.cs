using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Departments.Commands.UpdateDepartment;

public sealed record UpdateDepartmentCommand(
    Guid Id,
    Guid OrganizationId,
    string Code,
    string Name,
    string? Description,
    bool IsActive)
    : IRequest<Result<UpdateDepartmentResponse>>;
