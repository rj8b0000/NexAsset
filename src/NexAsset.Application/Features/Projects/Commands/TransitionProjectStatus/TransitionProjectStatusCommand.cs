using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Projects.Queries.GetProjects;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Projects.Commands.TransitionProjectStatus;

public sealed record TransitionProjectStatusCommand(
    Guid Id,
    ProjectStatus NewStatus,
    string? Remarks = null) : IRequest<Result<ProjectResponse>>;
