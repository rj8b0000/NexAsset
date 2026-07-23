using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Projects;

public sealed class ProjectCategoryApiClient : ApiClientBase, IProjectCategoryApiClient
{
    private const string BasePath = "api/project-categories";

    public ProjectCategoryApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<ApiResult<PagedResult<ProjectCategoryListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
        => GetAsync<PagedResult<ProjectCategoryListItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

    public Task<ApiResult<ProjectCategoryListItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetAsync<ProjectCategoryListItem>($"{BasePath}/{id}", cancellationToken);

    public Task<ApiResult<ProjectCategoryListItem>> CreateAsync(ProjectCategoryFormModel model, CancellationToken cancellationToken = default)
        => PostAsync<ProjectCategoryFormModel, ProjectCategoryListItem>(BasePath, model, cancellationToken);

    public Task<ApiResult<ProjectCategoryListItem>> UpdateAsync(Guid id, ProjectCategoryFormModel model, CancellationToken cancellationToken = default)
        => PutAsync<ProjectCategoryFormModel, ProjectCategoryListItem>($"{BasePath}/{id}", model, cancellationToken);

    public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => DeleteAsync($"{BasePath}/{id}", cancellationToken);
}
