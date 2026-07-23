using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectDocuments.Queries.GetDocuments;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.ProjectDocuments.Commands.UploadDocument;

public sealed record UploadDocumentCommand(
    Guid ProjectId,
    Guid? UploadedByEmployeeId,
    string DocumentName,
    DocumentCategory Category,
    string? Description,
    string FileReference,
    string? Remarks,
    DateOnly? ExpiryDate) : IRequest<Result<DocumentResponse>>;
