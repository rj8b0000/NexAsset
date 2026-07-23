using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectDocuments.Commands.DeleteDocument;

public sealed class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Result>
{
    private readonly IProjectDocumentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDocumentCommandHandler(IProjectDocumentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        var doc = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (doc == null)
        {
            return Result.Failure("Document not found.");
        }

        doc.IsDeleted = true;
        doc.DeletedAtUtc = DateTime.UtcNow;

        _repository.Update(doc);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
