using System.Collections.Generic;
using System.Threading.Tasks;
using NexAsset.Web.Models.Mock;

namespace NexAsset.Web.Infrastructure.Services
{
    public interface IAuditLogApiClient
    {
        Task<List<AuditLogMock>> GetAuditLogsAsync();
    }
}
