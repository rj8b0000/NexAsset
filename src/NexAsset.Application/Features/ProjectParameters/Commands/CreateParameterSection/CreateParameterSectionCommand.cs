using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectParameters.Queries.GetParameterSections;

namespace NexAsset.Application.Features.ProjectParameters.Commands.CreateParameterSection;

public sealed record CreateParameterSectionCommand(
    Guid ProjectId,
    string Name,
    int DisplayOrder) : IRequest<Result<ParameterSectionResponse>>;
