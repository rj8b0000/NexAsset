using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexAsset.Web.Models.Mock;
using NexAsset.Web.Infrastructure.Services;

namespace NexAsset.Web.Infrastructure.Services.Mock
{
    /// <summary>
    /// In-memory implementation of <see cref="IAuditLogApiClient"/> backed by <see cref="MockDatabaseService"/>.
    /// Temporary placeholder pending migration to the real NexAsset.API HTTP client.
    /// </summary>
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
