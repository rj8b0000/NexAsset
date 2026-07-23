using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Projects;

public sealed class ProjectDocumentApiClient : ApiClientBase, IProjectDocumentApiClient
{
    public ProjectDocumentApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<ApiResult<PagedResult<ProjectDocumentItem>>> GetPagedAsync(Guid projectId, DocumentCategory? category, PagedQuery query, CancellationToken cancellationToken = default)
    {
        var categoryParam = category.HasValue ? $"&category={(int)category.Value}" : string.Empty;
        var url = $"api/projects/{projectId}/documents?pageNumber={query.PageNumber}&pageSize={query.PageSize}&search={Uri.EscapeDataString(query.Search ?? "")}{categoryParam}";
        return GetAsync<PagedResult<ProjectDocumentItem>>(url, cancellationToken);
    }

    public Task<ApiResult> DeleteAsync(Guid projectId, Guid documentId, CancellationToken cancellationToken = default)
        => DeleteAsync($"api/projects/{projectId}/documents/{documentId}", cancellationToken);

    public Task<ApiResult<List<ProjectDocumentItem>>> GetHistoryAsync(Guid projectId, string documentName, CancellationToken cancellationToken = default)
        => GetAsync<List<ProjectDocumentItem>>($"api/projects/{projectId}/documents/history?documentName={Uri.EscapeDataString(documentName)}", cancellationToken);
}
