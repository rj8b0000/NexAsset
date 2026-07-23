using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectDocuments.Queries.GetDocuments;

public sealed class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQuery, Result<PagedResponse<DocumentResponse>>>
{
    private readonly IProjectDocumentRepository _repository;

    public GetDocumentsQueryHandler(IProjectDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResponse<DocumentResponse>>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        var pagedResult = await _repository.GetPagedAsync(request.ProjectId, request.Category, request.Search, request, cancellationToken);
        var items = pagedResult.Items.Select(doc => new DocumentResponse(
            doc.Id, doc.ProjectId, doc.DocumentName, doc.Category, doc.Description, doc.FileReference,
            doc.UploadedAtUtc, doc.Version, doc.Remarks, doc.ExpiryDate, doc.UploadedByEmployeeId,
            doc.UploadedByEmployee?.FirstName)).ToList();

        var response = new PagedResponse<DocumentResponse>
        {
            Items = items,
            TotalCount = pagedResult.TotalCount,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize
        };

        return Result<PagedResponse<DocumentResponse>>.Success(response);
    }
}
