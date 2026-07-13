using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Departments.Commands.CreateDepartment;

public sealed record CreateDepartmentCommand(
    Guid OrganizationId,
    string Code,
    string Name,
    string? Description)
    : IRequest<Result<CreateDepartmentResponse>>;
