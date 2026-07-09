using System.Collections.Generic;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.Models;

namespace NexAsset.Web.Infrastructure.Services
{
    public interface IEmployeeApiClient
    {
        Task<List<EmployeeMock>> GetEmployeesAsync();
        Task<EmployeeMock?> GetEmployeeAsync(string id);
        Task CreateEmployeeAsync(EmployeeMock employee);
        Task UpdateEmployeeAsync(EmployeeMock employee);
        Task DeleteEmployeeAsync(string id);
    }
}
