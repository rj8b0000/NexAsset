using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectDocuments.Queries.GetDocuments;

namespace NexAsset.Application.Features.ProjectDocuments.Queries.GetDocumentVersionHistory;

public sealed record GetDocumentVersionHistoryQuery(Guid ProjectId, string DocumentName) : IRequest<Result<List<DocumentResponse>>>;
