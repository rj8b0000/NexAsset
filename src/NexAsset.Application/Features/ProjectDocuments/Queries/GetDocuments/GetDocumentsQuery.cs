using MediatR;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.ProjectDocuments.Queries.GetDocuments;

public sealed class GetDocumentsQuery : PagedRequest, IRequest<Result<PagedResponse<DocumentResponse>>>
{
    public Guid ProjectId { get; init; }
    public DocumentCategory? Category { get; init; }
}
