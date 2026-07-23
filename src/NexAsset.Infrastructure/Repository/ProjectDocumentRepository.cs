using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Repository;

public sealed class ProjectDocumentRepository : IProjectDocumentRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectDocumentRepository(ApplicationDbContext context) => _context = context;

    public async Task AddAsync(ProjectDocument document, CancellationToken cancellationToken) =>
        await _context.ProjectDocuments.AddAsync(document, cancellationToken);

    public Task<ProjectDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _context.ProjectDocuments
            .Include(x => x.UploadedByEmployee)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public async Task<PagedResponse<ProjectDocument>> GetPagedAsync(
        Guid projectId,
        DocumentCategory? category,
        string? search,
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        IQueryable<ProjectDocument> query = _context.ProjectDocuments
            .Include(x => x.UploadedByEmployee)
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId && !x.IsDeleted);

        if (category.HasValue) query = query.Where(x => x.Category == category.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(x => x.DocumentName.ToLower().Contains(s) || (x.Description != null && x.Description.ToLower().Contains(s)));
        }

        query = request.SortBy?.ToLower() switch
        {
            "category" => request.Descending ? query.OrderByDescending(x => x.Category) : query.OrderBy(x => x.Category),
            "version" => request.Descending ? query.OrderByDescending(x => x.Version) : query.OrderBy(x => x.Version),
            "uploadedat" or "uploadedatutc" => request.Descending ? query.OrderByDescending(x => x.UploadedAtUtc) : query.OrderBy(x => x.UploadedAtUtc),
            _ => request.Descending ? query.OrderByDescending(x => x.DocumentName) : query.OrderBy(x => x.DocumentName)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync(cancellationToken);

        return new PagedResponse<ProjectDocument>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public Task<List<ProjectDocument>> GetVersionHistoryAsync(Guid projectId, string documentName, CancellationToken cancellationToken) =>
        _context.ProjectDocuments
            .Include(x => x.UploadedByEmployee)
            .Where(x => x.ProjectId == projectId && x.DocumentName.ToLower() == documentName.ToLower() && !x.IsDeleted)
            .OrderByDescending(x => x.Version)
            .ToListAsync(cancellationToken);

    public void Update(ProjectDocument document) => _context.ProjectDocuments.Update(document);
}
