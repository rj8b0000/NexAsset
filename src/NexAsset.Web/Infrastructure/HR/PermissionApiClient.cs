using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Models.HR;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.HR
{
    /// <summary>Real HTTP client for /api/permissions and role↔permission mapping.</summary>
    public sealed class PermissionApiClient : ApiClientBase, IPermissionApiClient
    {
        private const string BasePath = "api/permissions";

        public PermissionApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<PermissionListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<PermissionListItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<PermissionDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<PermissionDetail>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(PermissionFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<PermissionFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, PermissionFormModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<List<PermissionListItem>>> GetForRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
            => GetAsync<List<PermissionListItem>>($"{BasePath}/roles/{roleId}", cancellationToken);

        public Task<ApiResult> AssignToRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
            => PostAsync($"{BasePath}/roles/assign", new AssignPermissionToRoleRequest(roleId, permissionId), cancellationToken);

        public Task<ApiResult> RemoveFromRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/roles/{roleId}/{permissionId}", cancellationToken);
    }
}
