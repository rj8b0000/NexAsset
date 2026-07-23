using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Projects;

public interface IProjectDocumentApiClient
{
    Task<ApiResult<PagedResult<ProjectDocumentItem>>> GetPagedAsync(Guid projectId, DocumentCategory? category, PagedQuery query, CancellationToken cancellationToken = default);
    Task<ApiResult> DeleteAsync(Guid projectId, Guid documentId, CancellationToken cancellationToken = default);
    Task<ApiResult<List<ProjectDocumentItem>>> GetHistoryAsync(Guid projectId, string documentName, CancellationToken cancellationToken = default);
}
