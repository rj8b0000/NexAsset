using System.Collections.Generic;
using System.Threading.Tasks;
using NexAsset.Web.Models.Mock;

namespace NexAsset.Web.Infrastructure.Services
{
    public interface IMaintenanceApiClient
    {
        Task<List<MaintenanceMock>> GetMaintenanceTicketsAsync();
        Task<MaintenanceMock?> GetMaintenanceTicketAsync(string id);
        Task CreateMaintenanceTicketAsync(MaintenanceMock ticket);
        Task UpdateMaintenanceTicketAsync(MaintenanceMock ticket);
        Task DeleteMaintenanceTicketAsync(string id);
        Task ResolveMaintenanceTicketAsync(string id);
    }
}
