using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Foundation
{
    /// <summary>Typed client for /api/branches.</summary>
    public interface IBranchApiClient
    {
        Task<ApiResult<PagedResult<BranchListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<BranchDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> CreateAsync(BranchFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateAsync(Guid id, BranchFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
