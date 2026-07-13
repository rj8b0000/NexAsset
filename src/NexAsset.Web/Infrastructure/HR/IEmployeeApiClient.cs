using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Models.HR;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.HR
{
    /// <summary>
    /// Real typed client for /api/employees. Deliberately in the Infrastructure.HR namespace and
    /// referenced fully-qualified by the Employee pages, because the legacy mock
    /// <c>NexAsset.Web.Infrastructure.Services.IEmployeeApiClient</c> must remain for the
    /// out-of-scope Asset pages that still depend on it.
    /// </summary>
    public interface IEmployeeApiClient
    {
        Task<ApiResult<PagedResult<EmployeeListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<EmployeeDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> CreateAsync(EmployeeFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateAsync(Guid id, EmployeeFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
