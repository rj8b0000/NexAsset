using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Projects;

public sealed class ProjectCategoryApiClient(HttpClient httpClient) : ApiClientBase(httpClient), IProjectCategoryApiClient
{
    private const string BasePath = "api/project-categories";
    public Task<ApiResult<PagedResult<ProjectCategoryItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default) => GetAsync<PagedResult<ProjectCategoryItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);
    public Task<ApiResult<ProjectCategoryItem>> CreateAsync(ProjectCategoryInput input, CancellationToken cancellationToken = default) => PostAsync<ProjectCategoryInput, ProjectCategoryItem>(BasePath, input, cancellationToken);
    public Task<ApiResult<ProjectCategoryItem>> UpdateAsync(Guid id, ProjectCategoryInput input, CancellationToken cancellationToken = default) => PutAsync<ProjectCategoryInput, ProjectCategoryItem>($"{BasePath}/{id}", input, cancellationToken);
    public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default) => DeleteAsync($"{BasePath}/{id}", cancellationToken);
}
