using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Foundation
{
    /// <summary>Typed client for /api/designations.</summary>
    public interface IDesignationApiClient
    {
        Task<ApiResult<PagedResult<DesignationListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<DesignationDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> CreateAsync(DesignationFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateAsync(Guid id, DesignationFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
