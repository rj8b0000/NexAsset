using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Projects.Queries.GetProjects;

namespace NexAsset.Application.Features.Projects.Queries.GetProject;

public sealed record GetProjectQuery(Guid Id) : IRequest<Result<ProjectResponse>>;
