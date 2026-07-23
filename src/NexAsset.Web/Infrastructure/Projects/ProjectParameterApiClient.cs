using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;

namespace NexAsset.Web.Infrastructure.Projects;

public sealed class ProjectParameterApiClient : ApiClientBase, IProjectParameterApiClient
{
    public ProjectParameterApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<ApiResult<List<ParameterSectionModel>>> GetSectionsAsync(Guid projectId, CancellationToken cancellationToken = default)
        => GetAsync<List<ParameterSectionModel>>($"api/projects/{projectId}/parameters", cancellationToken);

    public Task<ApiResult<ParameterSectionModel>> CreateSectionAsync(Guid projectId, ParameterSectionModel model, CancellationToken cancellationToken = default)
        => PostAsync<ParameterSectionModel, ParameterSectionModel>($"api/projects/{projectId}/parameters/sections", model, cancellationToken);

    public Task<ApiResult> UpdateSectionAsync(Guid projectId, Guid sectionId, ParameterSectionModel model, CancellationToken cancellationToken = default)
        => PutAsync($"api/projects/{projectId}/parameters/sections/{sectionId}", model, cancellationToken);

    public Task<ApiResult> DeleteSectionAsync(Guid projectId, Guid sectionId, CancellationToken cancellationToken = default)
        => DeleteAsync($"api/projects/{projectId}/parameters/sections/{sectionId}", cancellationToken);

    public Task<ApiResult<ParameterFieldModel>> AddFieldAsync(Guid projectId, Guid sectionId, ParameterFieldModel model, CancellationToken cancellationToken = default)
        => PostAsync<ParameterFieldModel, ParameterFieldModel>($"api/projects/{projectId}/parameters/sections/{sectionId}/fields", model, cancellationToken);

    public Task<ApiResult<ParameterFieldModel>> UpdateFieldAsync(Guid projectId, Guid fieldId, ParameterFieldModel model, CancellationToken cancellationToken = default)
        => PutAsync<ParameterFieldModel, ParameterFieldModel>($"api/projects/{projectId}/parameters/fields/{fieldId}", model, cancellationToken);

    public Task<ApiResult> DeleteFieldAsync(Guid projectId, Guid fieldId, CancellationToken cancellationToken = default)
        => DeleteAsync($"api/projects/{projectId}/parameters/fields/{fieldId}", cancellationToken);

    public Task<ApiResult> SaveValuesAsync(Guid projectId, List<ParameterValueItemModel> values, CancellationToken cancellationToken = default)
        => PostAsync<object>($"api/projects/{projectId}/parameters/values", new { ProjectId = projectId, Values = values }, cancellationToken);
}
