using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectParameters.Queries.GetParameterSections;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.ProjectParameters.Commands.UpdateParameter;

public sealed record UpdateParameterCommand(
    Guid Id,
    string ParameterName,
    ParameterInputType InputType,
    string? Unit,
    bool IsRequired,
    int DisplayOrder,
    string? DropdownOptionsJson) : IRequest<Result<ParameterResponse>>;
