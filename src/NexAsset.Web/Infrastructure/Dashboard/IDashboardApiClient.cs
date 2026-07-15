using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Dashboard;

namespace NexAsset.Web.Infrastructure.Dashboard
{
    /// <summary>Typed client for the per-organization dashboard summary.</summary>
    public interface IDashboardApiClient
    {
        Task<ApiResult<DashboardSummary>> GetSummaryAsync(Guid organizationId, CancellationToken cancellationToken = default);
    }
}
