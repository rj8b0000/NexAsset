using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectCategories.Commands.DeleteProjectCategory;

public sealed record DeleteProjectCategoryCommand(Guid Id) : IRequest<Result>;
