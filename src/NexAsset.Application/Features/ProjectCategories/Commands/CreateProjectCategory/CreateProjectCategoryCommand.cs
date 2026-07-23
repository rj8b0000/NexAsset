using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategories;

namespace NexAsset.Application.Features.ProjectCategories.Commands.CreateProjectCategory;

public sealed record CreateProjectCategoryCommand(
    Guid OrganizationId,
    string Name,
    string? Description,
    bool IsActive = true) : IRequest<Result<ProjectCategoryResponse>>;
