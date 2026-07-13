namespace NexAsset.Application.Features.Designations.Commands.UpdateDesignation;

public sealed record UpdateDesignationResponse(
    Guid Id,
    string Title,
    bool IsActive);
