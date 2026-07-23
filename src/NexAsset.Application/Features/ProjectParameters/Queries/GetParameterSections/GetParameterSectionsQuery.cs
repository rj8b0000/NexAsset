using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectParameters.Queries.GetParameterSections;

namespace NexAsset.Application.Features.ProjectParameters.Queries.GetParameterSections;

public sealed record GetParameterSectionsQuery(Guid ProjectId) : IRequest<Result<List<ParameterSectionResponse>>>;
