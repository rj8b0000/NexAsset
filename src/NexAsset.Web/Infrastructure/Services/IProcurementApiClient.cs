using System.Collections.Generic;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.Models;

namespace NexAsset.Web.Infrastructure.Services
{
    public interface IProcurementApiClient
    {
        Task<List<ProcurementMock>> GetProcurementRequestsAsync();
        Task<ProcurementMock?> GetProcurementRequestAsync(string id);
        Task CreateProcurementRequestAsync(ProcurementMock pr);
        Task UpdateProcurementRequestAsync(ProcurementMock pr);
        Task DeleteProcurementRequestAsync(string id);
        Task ApproveProcurementRequestAsync(string id);
        Task RejectProcurementRequestAsync(string id);
    }
}
