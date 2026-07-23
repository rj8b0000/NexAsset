using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Projects;

namespace NexAsset.Web.Infrastructure.Projects;

public interface IProjectParameterApiClient
{
    Task<ApiResult<List<ParameterSectionModel>>> GetSectionsAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<ApiResult<ParameterSectionModel>> CreateSectionAsync(Guid projectId, ParameterSectionModel model, CancellationToken cancellationToken = default);
    Task<ApiResult> UpdateSectionAsync(Guid projectId, Guid sectionId, ParameterSectionModel model, CancellationToken cancellationToken = default);
    Task<ApiResult> DeleteSectionAsync(Guid projectId, Guid sectionId, CancellationToken cancellationToken = default);
    Task<ApiResult<ParameterFieldModel>> AddFieldAsync(Guid projectId, Guid sectionId, ParameterFieldModel model, CancellationToken cancellationToken = default);
    Task<ApiResult<ParameterFieldModel>> UpdateFieldAsync(Guid projectId, Guid fieldId, ParameterFieldModel model, CancellationToken cancellationToken = default);
    Task<ApiResult> DeleteFieldAsync(Guid projectId, Guid fieldId, CancellationToken cancellationToken = default);
    Task<ApiResult> SaveValuesAsync(Guid projectId, List<ParameterValueItemModel> values, CancellationToken cancellationToken = default);
}
