using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Models.HR;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.HR
{
    /// <summary>Real HTTP client for /api/employees.</summary>
    public sealed class EmployeeApiClient : ApiClientBase, IEmployeeApiClient
    {
        private const string BasePath = "api/employees";

        public EmployeeApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<EmployeeListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<EmployeeListItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<EmployeeDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<EmployeeDetail>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(EmployeeFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<EmployeeFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, EmployeeFormModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/{id}", cancellationToken);
    }
}
