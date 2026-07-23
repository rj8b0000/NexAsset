using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Projects.Queries.GetDraftSession;

namespace NexAsset.Application.Features.Projects.Queries.GetDraftSession;

public sealed record GetDraftSessionQuery(Guid UserId, Guid OrganizationId) : IRequest<Result<DraftSessionResponse?>>;
