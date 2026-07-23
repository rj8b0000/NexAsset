using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectDocuments.Queries.GetDocuments;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Features.ProjectDocuments.Commands.UploadDocument;

public sealed class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, Result<DocumentResponse>>
{
    private readonly IProjectDocumentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadDocumentCommandHandler(IProjectDocumentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DocumentResponse>> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        var doc = new ProjectDocument
        {
            ProjectId = request.ProjectId,
            UploadedByEmployeeId = request.UploadedByEmployeeId,
            DocumentName = request.DocumentName,
            Category = request.Category,
            Description = request.Description,
            FileReference = request.FileReference,
            Remarks = request.Remarks,
            ExpiryDate = request.ExpiryDate,
            Version = 1,
            UploadedAtUtc = DateTime.UtcNow
        };

        await _repository.AddAsync(doc, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var loaded = await _repository.GetByIdAsync(doc.Id, cancellationToken);
        return Result<DocumentResponse>.Success(new DocumentResponse(
            doc.Id, doc.ProjectId, doc.DocumentName, doc.Category, doc.Description, doc.FileReference,
            doc.UploadedAtUtc, doc.Version, doc.Remarks, doc.ExpiryDate, doc.UploadedByEmployeeId,
            loaded?.UploadedByEmployee?.FirstName));
    }
}
