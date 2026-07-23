using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectParameters.Commands.UpdateParameterSection;

public sealed record UpdateParameterSectionCommand(Guid Id, string Name, int DisplayOrder) : IRequest<Result>;
