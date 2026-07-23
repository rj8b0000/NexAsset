using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Projects.Queries.GetDraftSession;

public sealed class GetDraftSessionQueryHandler : IRequestHandler<GetDraftSessionQuery, Result<DraftSessionResponse?>>
{
    private readonly IDraftSessionRepository _repository;

    public GetDraftSessionQueryHandler(IDraftSessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<DraftSessionResponse?>> Handle(GetDraftSessionQuery request, CancellationToken cancellationToken)
    {
        var draft = await _repository.GetByUserAsync(request.UserId, request.OrganizationId, cancellationToken);
        if (draft == null)
        {
            return Result<DraftSessionResponse?>.Success(null);
        }

        return Result<DraftSessionResponse?>.Success(new DraftSessionResponse(
            draft.Id, draft.UserId, draft.OrganizationId, draft.WizardStateJson, draft.CurrentStep, draft.LastSavedAtUtc));
    }
}
