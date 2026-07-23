using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.ProjectDocuments.Queries.GetDocuments;

public sealed record DocumentResponse(
    Guid Id,
    Guid ProjectId,
    string DocumentName,
    DocumentCategory Category,
    string? Description,
    string FileReference,
    DateTime UploadedAtUtc,
    int Version,
    string? Remarks,
    DateOnly? ExpiryDate,
    Guid? UploadedByEmployeeId,
    string? UploadedByEmployeeName);
