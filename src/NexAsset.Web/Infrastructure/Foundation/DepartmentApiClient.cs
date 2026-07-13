using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Foundation
{
    /// <summary>Real HTTP client for /api/departments.</summary>
    public sealed class DepartmentApiClient : ApiClientBase, IDepartmentApiClient
    {
        private const string BasePath = "api/departments";

        public DepartmentApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<DepartmentListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<DepartmentListItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<DepartmentDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<DepartmentDetail>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(DepartmentFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<DepartmentFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, DepartmentFormModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/{id}", cancellationToken);
    }
}
