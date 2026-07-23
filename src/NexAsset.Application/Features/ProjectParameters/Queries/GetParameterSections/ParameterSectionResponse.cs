using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.ProjectParameters.Queries.GetParameterSections;

public sealed record ParameterSectionResponse(
    Guid Id,
    Guid ProjectId,
    string Name,
    int DisplayOrder,
    List<ParameterResponse> Parameters);

public sealed record ParameterResponse(
    Guid Id,
    Guid SectionId,
    string ParameterName,
    ParameterInputType InputType,
    string? Unit,
    bool IsRequired,
    int DisplayOrder,
    string? DropdownOptionsJson);

public sealed record ParameterValueItem(
    Guid ParameterId,
    string? Value);
