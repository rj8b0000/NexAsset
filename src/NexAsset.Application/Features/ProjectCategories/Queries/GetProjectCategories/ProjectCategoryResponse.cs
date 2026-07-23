namespace NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategories;

public sealed record ProjectCategoryResponse(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string? Description,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
