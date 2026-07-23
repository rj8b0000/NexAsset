using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectParameters.Queries.GetParameterSections;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.ProjectParameters.Commands.AddParameter;

public sealed record AddParameterCommand(
    Guid SectionId,
    string ParameterName,
    ParameterInputType InputType,
    string? Unit,
    bool IsRequired,
    int DisplayOrder,
    string? DropdownOptionsJson) : IRequest<Result<ParameterResponse>>;
