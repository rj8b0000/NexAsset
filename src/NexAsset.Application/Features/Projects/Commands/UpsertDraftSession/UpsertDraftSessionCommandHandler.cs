using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Projects.Queries.GetDraftSession;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Features.Projects.Commands.UpsertDraftSession;

public sealed class UpsertDraftSessionCommandHandler : IRequestHandler<UpsertDraftSessionCommand, Result<DraftSessionResponse>>
{
    private readonly IDraftSessionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpsertDraftSessionCommandHandler(IDraftSessionRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DraftSessionResponse>> Handle(UpsertDraftSessionCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByUserAsync(request.UserId, request.OrganizationId, cancellationToken);
        if (existing != null)
        {
            existing.WizardStateJson = request.WizardStateJson;
            existing.CurrentStep = request.CurrentStep;
            existing.LastSavedAtUtc = DateTime.UtcNow;
            existing.UpdatedAtUtc = DateTime.UtcNow;

            _repository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<DraftSessionResponse>.Success(new DraftSessionResponse(
                existing.Id, existing.UserId, existing.OrganizationId, existing.WizardStateJson, existing.CurrentStep, existing.LastSavedAtUtc));
        }

        var newDraft = new DraftSession
        {
            UserId = request.UserId,
            OrganizationId = request.OrganizationId,
            WizardStateJson = request.WizardStateJson,
            CurrentStep = request.CurrentStep,
            LastSavedAtUtc = DateTime.UtcNow
        };

        await _repository.AddAsync(newDraft, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<DraftSessionResponse>.Success(new DraftSessionResponse(
            newDraft.Id, newDraft.UserId, newDraft.OrganizationId, newDraft.WizardStateJson, newDraft.CurrentStep, newDraft.LastSavedAtUtc));
    }
}
