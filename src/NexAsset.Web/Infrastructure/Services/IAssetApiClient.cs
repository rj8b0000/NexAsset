using System.Collections.Generic;
using System.Threading.Tasks;
using NexAsset.Web.Models.Mock;

namespace NexAsset.Web.Infrastructure.Services
{
    public interface IAssetApiClient
    {
        Task<List<AssetMock>> GetAssetsAsync();
        Task<AssetMock?> GetAssetAsync(string id);
        Task CreateAssetAsync(AssetMock asset);
        Task UpdateAssetAsync(AssetMock asset);
        Task DeleteAssetAsync(string id);
        Task AssignAssetAsync(string id, string employeeName);
        Task ReturnAssetAsync(string id);
        Task TransferAssetAsync(string id, string targetBranch);
        Task SetMaintenanceStatusAsync(string id, bool inMaintenance);
    }
}
