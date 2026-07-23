using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectParameters.Commands.DeleteParameterSection;

public sealed record DeleteParameterSectionCommand(Guid Id) : IRequest<Result>;
