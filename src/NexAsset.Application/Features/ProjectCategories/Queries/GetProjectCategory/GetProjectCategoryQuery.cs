using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategories;

namespace NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategory;

public sealed record GetProjectCategoryQuery(Guid Id) : IRequest<Result<ProjectCategoryResponse>>;
