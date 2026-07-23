using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Projects.Queries.GetDraftSession;

namespace NexAsset.Application.Features.Projects.Commands.UpsertDraftSession;

public sealed record UpsertDraftSessionCommand(
    Guid UserId,
    Guid OrganizationId,
    string WizardStateJson,
    int CurrentStep) : IRequest<Result<DraftSessionResponse>>;
