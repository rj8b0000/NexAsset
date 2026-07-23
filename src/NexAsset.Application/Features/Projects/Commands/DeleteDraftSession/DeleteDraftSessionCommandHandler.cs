using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Projects.Commands.DeleteDraftSession;

public sealed class DeleteDraftSessionCommandHandler : IRequestHandler<DeleteDraftSessionCommand, Result>
{
    private readonly IDraftSessionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDraftSessionCommandHandler(IDraftSessionRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteDraftSessionCommand request, CancellationToken cancellationToken)
    {
        var draft = await _repository.GetByUserAsync(request.UserId, request.OrganizationId, cancellationToken);
        if (draft != null)
        {
            _repository.Remove(draft);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}
