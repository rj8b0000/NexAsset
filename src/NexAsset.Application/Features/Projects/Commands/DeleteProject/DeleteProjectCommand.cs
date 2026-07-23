using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Projects.Commands.DeleteProject;

public sealed record DeleteProjectCommand(Guid Id) : IRequest<Result>;
