using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Dashboard;

namespace NexAsset.Web.Infrastructure.Dashboard
{
    /// <summary>Real HTTP client for /api/enterprise-operations/dashboard/{organizationId}.</summary>
    public sealed class DashboardApiClient : ApiClientBase, IDashboardApiClient
    {
        public DashboardApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<DashboardSummary>> GetSummaryAsync(Guid organizationId, CancellationToken cancellationToken = default)
            => GetAsync<DashboardSummary>($"api/enterprise-operations/dashboard/{organizationId}", cancellationToken);
    }
}
