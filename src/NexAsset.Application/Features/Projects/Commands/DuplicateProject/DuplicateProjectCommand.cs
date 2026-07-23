using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Projects.Queries.GetProjects;

namespace NexAsset.Application.Features.Projects.Commands.DuplicateProject;

public sealed record DuplicateProjectCommand(Guid SourceProjectId, Guid OrganizationId) : IRequest<Result<ProjectResponse>>;
