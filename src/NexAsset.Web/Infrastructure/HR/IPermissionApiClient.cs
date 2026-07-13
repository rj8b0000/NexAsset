using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Models.HR;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.HR
{
    /// <summary>Typed client for /api/permissions, including role↔permission mapping.</summary>
    public interface IPermissionApiClient
    {
        Task<ApiResult<PagedResult<PermissionListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<PermissionDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> CreateAsync(PermissionFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateAsync(Guid id, PermissionFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        // Role ↔ permission mapping
        Task<ApiResult<List<PermissionListItem>>> GetForRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
        Task<ApiResult> AssignToRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default);
        Task<ApiResult> RemoveFromRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default);
    }
}
