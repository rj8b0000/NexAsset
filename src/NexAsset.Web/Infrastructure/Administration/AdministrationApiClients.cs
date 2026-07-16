using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Administration;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Administration
{
    // Typed clients for the administration endpoints under /api/enterprise-operations.
    // Grouped in one file — each is a thin two/three-method client over ApiClientBase.
    // IAuditTrailApiClient is named to avoid colliding with the legacy mock
    // Infrastructure.Services.IAuditLogApiClient during the migration window.

    /// <summary>Server notifications: paged list + mark-as-read.</summary>
    public interface INotificationApiClient
    {
        Task<ApiResult<PagedResult<ServerNotification>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult> MarkReadAsync(Guid id, CancellationToken cancellationToken = default);
    }

    public sealed class NotificationApiClient : ApiClientBase, INotificationApiClient
    {
        private const string BasePath = "api/enterprise-operations/notifications";

        public NotificationApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<ServerNotification>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<ServerNotification>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult> MarkReadAsync(Guid id, CancellationToken cancellationToken = default)
            => PostAsync($"{BasePath}/{id}/read", cancellationToken);
    }

    /// <summary>Audit trail: paged read-only list (logs are written server-side).</summary>
    public interface IAuditTrailApiClient
    {
        Task<ApiResult<PagedResult<AuditLogRecord>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
    }

    public sealed class AuditTrailApiClient : ApiClientBase, IAuditTrailApiClient
    {
        private const string BasePath = "api/enterprise-operations/audit-logs";

        public AuditTrailApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<AuditLogRecord>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<AuditLogRecord>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);
    }

    /// <summary>System settings: paged list + key-based upsert.</summary>
    public interface ISystemSettingApiClient
    {
        Task<ApiResult<PagedResult<SystemSettingRecord>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> UpsertAsync(SystemSettingFormModel model, CancellationToken cancellationToken = default);
    }

    public sealed class SystemSettingApiClient : ApiClientBase, ISystemSettingApiClient
    {
        private const string BasePath = "api/enterprise-operations/system-settings";

        public SystemSettingApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<SystemSettingRecord>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<SystemSettingRecord>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<CreatedResponse>> UpsertAsync(SystemSettingFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<SystemSettingFormModel, CreatedResponse>(BasePath, model, cancellationToken);
    }
}
