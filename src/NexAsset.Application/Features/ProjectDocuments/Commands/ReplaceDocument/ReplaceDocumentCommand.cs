using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectDocuments.Queries.GetDocuments;

namespace NexAsset.Application.Features.ProjectDocuments.Commands.ReplaceDocument;

public sealed record ReplaceDocumentCommand(
    Guid Id,
    string FileReference,
    string? Remarks,
    Guid? UploadedByEmployeeId) : IRequest<Result<DocumentResponse>>;
