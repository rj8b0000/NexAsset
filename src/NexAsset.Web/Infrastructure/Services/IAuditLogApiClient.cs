using System.Collections.Generic;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.Models;

namespace NexAsset.Web.Infrastructure.Services
{
    public interface IAuditLogApiClient
    {
        Task<List<AuditLogMock>> GetAuditLogsAsync();
    }
}
