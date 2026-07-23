using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectDocuments.Queries.GetDocuments;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Features.ProjectDocuments.Commands.ReplaceDocument;

public sealed class ReplaceDocumentCommandHandler : IRequestHandler<ReplaceDocumentCommand, Result<DocumentResponse>>
{
    private readonly IProjectDocumentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ReplaceDocumentCommandHandler(IProjectDocumentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DocumentResponse>> Handle(ReplaceDocumentCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (existing == null)
        {
            return Result<DocumentResponse>.Failure("Document not found.");
        }

        var newVersionDoc = new ProjectDocument
        {
            ProjectId = existing.ProjectId,
            UploadedByEmployeeId = request.UploadedByEmployeeId ?? existing.UploadedByEmployeeId,
            DocumentName = existing.DocumentName,
            Category = existing.Category,
            Description = existing.Description,
            FileReference = request.FileReference,
            Remarks = request.Remarks ?? existing.Remarks,
            ExpiryDate = existing.ExpiryDate,
            Version = existing.Version + 1,
            UploadedAtUtc = DateTime.UtcNow
        };

        await _repository.AddAsync(newVersionDoc, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var loaded = await _repository.GetByIdAsync(newVersionDoc.Id, cancellationToken);
        return Result<DocumentResponse>.Success(new DocumentResponse(
            newVersionDoc.Id, newVersionDoc.ProjectId, newVersionDoc.DocumentName, newVersionDoc.Category,
            newVersionDoc.Description, newVersionDoc.FileReference, newVersionDoc.UploadedAtUtc, newVersionDoc.Version,
            newVersionDoc.Remarks, newVersionDoc.ExpiryDate, newVersionDoc.UploadedByEmployeeId, loaded?.UploadedByEmployee?.FirstName));
    }
}
