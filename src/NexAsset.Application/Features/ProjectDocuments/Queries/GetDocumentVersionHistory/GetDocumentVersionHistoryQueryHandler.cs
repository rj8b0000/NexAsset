using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectDocuments.Queries.GetDocuments;

namespace NexAsset.Application.Features.ProjectDocuments.Queries.GetDocumentVersionHistory;

public sealed class GetDocumentVersionHistoryQueryHandler : IRequestHandler<GetDocumentVersionHistoryQuery, Result<List<DocumentResponse>>>
{
    private readonly IProjectDocumentRepository _repository;

    public GetDocumentVersionHistoryQueryHandler(IProjectDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<DocumentResponse>>> Handle(GetDocumentVersionHistoryQuery request, CancellationToken cancellationToken)
    {
        var history = await _repository.GetVersionHistoryAsync(request.ProjectId, request.DocumentName, cancellationToken);
        var response = history.Select(doc => new DocumentResponse(
            doc.Id, doc.ProjectId, doc.DocumentName, doc.Category, doc.Description, doc.FileReference,
            doc.UploadedAtUtc, doc.Version, doc.Remarks, doc.ExpiryDate, doc.UploadedByEmployeeId,
            doc.UploadedByEmployee?.FirstName)).ToList();

        return Result<List<DocumentResponse>>.Success(response);
    }
}
