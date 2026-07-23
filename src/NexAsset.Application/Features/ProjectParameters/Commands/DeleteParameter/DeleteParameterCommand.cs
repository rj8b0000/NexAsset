using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectParameters.Commands.DeleteParameter;

public sealed record DeleteParameterCommand(Guid Id) : IRequest<Result>;
