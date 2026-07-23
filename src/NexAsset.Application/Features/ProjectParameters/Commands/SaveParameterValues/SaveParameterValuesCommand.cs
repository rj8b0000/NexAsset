using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectParameters.Queries.GetParameterSections;

namespace NexAsset.Application.Features.ProjectParameters.Commands.SaveParameterValues;

public sealed record SaveParameterValuesCommand(
    Guid ProjectId,
    List<ParameterValueItem> Values) : IRequest<Result>;
