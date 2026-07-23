using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Projects.Queries.GetProjects;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Projects.Commands.UpdateProject;

public sealed record UpdateProjectCommand(
    Guid Id,
    Guid OrganizationId,
    string ProjectCode,
    string ProjectName,
    string? Description,
    string? Notes,
    string? InternalRemarks,
    Guid CategoryId,
    Guid? CustomerId,
    Guid? BranchId,
    Guid? DepartmentId,
    Guid? ProjectManagerEmployeeId,
    ProjectPriority Priority,
    DateOnly? StartDate,
    DateOnly? EndDate,
    DateOnly? ExpectedCompletionDate) : IRequest<Result<ProjectResponse>>;
