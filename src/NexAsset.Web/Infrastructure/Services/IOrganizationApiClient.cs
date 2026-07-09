using System.Collections.Generic;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.Models;

namespace NexAsset.Web.Infrastructure.Services
{
    public interface IOrganizationApiClient
    {
        Task<List<OrganizationMock>> GetOrganizationsAsync();
        Task<OrganizationMock?> GetOrganizationAsync(string id);
        Task CreateOrganizationAsync(OrganizationMock org);
        Task UpdateOrganizationAsync(OrganizationMock org);
        Task DeleteOrganizationAsync(string id);
    }
}
