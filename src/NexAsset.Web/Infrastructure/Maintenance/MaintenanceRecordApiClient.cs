using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Models.Maintenance;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Maintenance
{
    /// <summary>Real HTTP client for /api/enterprise-operations/maintenance.</summary>
    public sealed class MaintenanceRecordApiClient : ApiClientBase, IMaintenanceRecordApiClient
    {
        private const string BasePath = "api/enterprise-operations/maintenance";

        public MaintenanceRecordApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<MaintenanceRecordItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<MaintenanceRecordItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<MaintenanceRecordItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<MaintenanceRecordItem>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(MaintenanceFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<MaintenanceFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> SetStatusAsync(Guid id, MaintenanceStatusRequest request, CancellationToken cancellationToken = default)
            => PostAsync($"{BasePath}/{id}/status", request, cancellationToken);
    }
}
