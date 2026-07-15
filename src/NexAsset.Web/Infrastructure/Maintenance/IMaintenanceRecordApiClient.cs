using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Models.Maintenance;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Maintenance
{
    /// <summary>
    /// Typed client for /api/enterprise-operations/maintenance. Workflow entity: create + status
    /// transitions only. Named MaintenanceRecord* to avoid colliding with the legacy mock
    /// <c>Infrastructure.Services.IMaintenanceApiClient</c> that _Imports.razor pulls in globally.
    /// </summary>
    public interface IMaintenanceRecordApiClient
    {
        Task<ApiResult<PagedResult<MaintenanceRecordItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<MaintenanceRecordItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> CreateAsync(MaintenanceFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> SetStatusAsync(Guid id, MaintenanceStatusRequest request, CancellationToken cancellationToken = default);
    }
}
