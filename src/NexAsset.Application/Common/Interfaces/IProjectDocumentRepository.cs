using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Common.Interfaces;

public interface IProjectDocumentRepository
{
    Task AddAsync(ProjectDocument document, CancellationToken cancellationToken);
    Task<ProjectDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResponse<ProjectDocument>> GetPagedAsync(
        Guid projectId,
        DocumentCategory? category,
        string? search,
        PagedRequest request,
        CancellationToken cancellationToken);
    Task<List<ProjectDocument>> GetVersionHistoryAsync(Guid projectId, string documentName, CancellationToken cancellationToken);
    void Update(ProjectDocument document);
}
