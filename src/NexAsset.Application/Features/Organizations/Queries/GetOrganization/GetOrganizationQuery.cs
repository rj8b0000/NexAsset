using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Organizations.Queries.GetOrganization;

public sealed record GetOrganizationQuery(Guid Id)
    : IRequest<Result<GetOrganizationResponse>>;