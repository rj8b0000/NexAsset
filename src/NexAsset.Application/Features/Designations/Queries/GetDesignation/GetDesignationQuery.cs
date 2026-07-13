using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Designations.Queries.GetDesignation;

public sealed record GetDesignationQuery(Guid Id)
    : IRequest<Result<GetDesignationResponse>>;
