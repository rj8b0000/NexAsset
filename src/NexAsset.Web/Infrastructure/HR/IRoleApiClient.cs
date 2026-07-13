using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.HR;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.HR
{
    /// <summary>Typed client for /api/roles (ASP.NET Identity roles: Id + Name only).</summary>
    public interface IRoleApiClient
    {
        Task<ApiResult<PagedResult<RoleItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<RoleItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<RoleItem>> CreateAsync(RoleFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateAsync(Guid id, RoleFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
