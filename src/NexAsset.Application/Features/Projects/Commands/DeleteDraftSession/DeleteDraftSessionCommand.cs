using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Projects.Commands.DeleteDraftSession;

public sealed record DeleteDraftSessionCommand(Guid UserId, Guid OrganizationId) : IRequest<Result>;
