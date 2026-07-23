using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategories;

namespace NexAsset.Application.Features.ProjectCategories.Commands.UpdateProjectCategory;

public sealed record UpdateProjectCategoryCommand(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string? Description,
    bool IsActive) : IRequest<Result<ProjectCategoryResponse>>;
