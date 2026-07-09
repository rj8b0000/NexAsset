using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.Models;

namespace NexAsset.Web.Infrastructure.Services
{
    public class AuditLogApiClient : IAuditLogApiClient
    {
        private readonly MockDatabaseService _db;

        public AuditLogApiClient(MockDatabaseService db)
        {
            _db = db;
        }

        public Task<List<AuditLogMock>> GetAuditLogsAsync()
        {
            return Task.FromResult(_db.AuditLogs.ToList());
        }
    }
}
