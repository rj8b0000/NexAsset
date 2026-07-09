using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.Models;
using NexAsset.Web.State;

namespace NexAsset.Web.Infrastructure.Services
{
    public class AssetApiClient : IAssetApiClient
    {
        private readonly MockDatabaseService _db;
        private readonly NotificationState _notifications;

        public AssetApiClient(MockDatabaseService db, NotificationState notifications)
        {
            _db = db;
            _notifications = notifications;
        }

        public Task<List<AssetMock>> GetAssetsAsync()
        {
            return Task.FromResult(_db.Assets.ToList());
        }

        public Task<AssetMock?> GetAssetAsync(string id)
        {
            return Task.FromResult(_db.Assets.FirstOrDefault(a => a.Id == id));
        }

        public Task CreateAssetAsync(AssetMock asset)
        {
            asset.Id = $"AST-{_db.Assets.Count + 101:D3}";
            asset.CreatedDate = DateTime.Now;
            asset.UpdatedDate = DateTime.Now;
            _db.Assets.Insert(0, asset);

            _db.AddAuditLog(asset.Id, "Asset", "Create", $"Created asset: {asset.Name}");
            _notifications.AddActivity("Asset Created", $"New asset {asset.Id} ({asset.Name}) was registered.");
            _notifications.AddToast("Asset Registered", $"Successfully registered asset {asset.Id}", ToastType.Success);
            
            return Task.CompletedTask;
        }

        public Task UpdateAssetAsync(AssetMock updatedAsset)
        {
            var idx = _db.Assets.FindIndex(a => a.Id == updatedAsset.Id);
            if (idx >= 0)
            {
                updatedAsset.UpdatedDate = DateTime.Now;
                _db.Assets[idx] = updatedAsset;
                
                _db.AddAuditLog(updatedAsset.Id, "Asset", "Update", $"Updated asset: {updatedAsset.Name}");
                _notifications.AddActivity("Asset Updated", $"Asset {updatedAsset.Id} metadata was modified.");
                _notifications.AddToast("Asset Updated", $"Updated asset details for {updatedAsset.Id}", ToastType.Success);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAssetAsync(string id)
        {
            var asset = _db.Assets.FirstOrDefault(a => a.Id == id);
            if (asset != null)
            {
                _db.Assets.Remove(asset);
                _db.AddAuditLog(id, "Asset", "Delete", $"Deleted asset: {asset.Name}");
                _notifications.AddActivity("Asset Deleted", $"Asset {id} was permanently removed.");
                _notifications.AddToast("Asset Deleted", $"Deleted asset {id}", ToastType.Warning);
            }
            return Task.CompletedTask;
        }

        public Task AssignAssetAsync(string assetId, string employeeName)
        {
            var asset = _db.Assets.FirstOrDefault(a => a.Id == assetId);
            if (asset != null)
            {
                var prevAssignee = asset.AssignedTo;
                asset.AssignedTo = employeeName;
                asset.Status = "Assigned";
                asset.UpdatedDate = DateTime.Now;
                
                // Adjust assetsAssigned count on employee if they exist in db
                var emp = _db.Employees.FirstOrDefault(e => e.Name == employeeName);
                if (emp != null) emp.AssetsAssigned++;

                if (!string.IsNullOrEmpty(prevAssignee))
                {
                    var prevEmp = _db.Employees.FirstOrDefault(e => e.Name == prevAssignee);
                    if (prevEmp != null && prevEmp.AssetsAssigned > 0) prevEmp.AssetsAssigned--;
                }
                
                _db.AddAuditLog(assetId, "Asset", "Assign", $"Assigned to {employeeName} (previously: {prevAssignee ?? "None"})");
                _notifications.AddActivity("Asset Assigned", $"Asset {assetId} assigned to {employeeName}.");
                _notifications.AddToast("Asset Assigned", $"Asset {assetId} is now assigned to {employeeName}", ToastType.Success);
            }
            return Task.CompletedTask;
        }

        public Task ReturnAssetAsync(string assetId)
        {
            var asset = _db.Assets.FirstOrDefault(a => a.Id == assetId);
            if (asset != null)
            {
                var prevAssignee = asset.AssignedTo;
                asset.AssignedTo = null;
                asset.Status = "Available";
                asset.UpdatedDate = DateTime.Now;

                // Adjust assetsAssigned count on employee if they exist in db
                if (!string.IsNullOrEmpty(prevAssignee))
                {
                    var emp = _db.Employees.FirstOrDefault(e => e.Name == prevAssignee);
                    if (emp != null && emp.AssetsAssigned > 0) emp.AssetsAssigned--;
                }
                
                _db.AddAuditLog(assetId, "Asset", "Return", $"Returned by {prevAssignee ?? "Unknown"}");
                _notifications.AddActivity("Asset Returned", $"Asset {assetId} was returned and is now available.");
                _notifications.AddToast("Asset Returned", $"Asset {assetId} is now Available", ToastType.Info);
            }
            return Task.CompletedTask;
        }

        public Task TransferAssetAsync(string assetId, string targetBranch)
        {
            var asset = _db.Assets.FirstOrDefault(a => a.Id == assetId);
            if (asset != null)
            {
                var prevLocation = asset.Location;
                asset.Location = targetBranch;
                asset.UpdatedDate = DateTime.Now;
                
                _db.AddAuditLog(assetId, "Asset", "Transfer", $"Transferred from {prevLocation} to {targetBranch}");
                _notifications.AddActivity("Asset Transferred", $"Asset {assetId} was transferred to {targetBranch}.");
                _notifications.AddToast("Asset Transferred", $"Transferred {assetId} to {targetBranch}", ToastType.Success);
            }
            return Task.CompletedTask;
        }

        public Task SetMaintenanceStatusAsync(string assetId, bool inMaintenance)
        {
            var asset = _db.Assets.FirstOrDefault(a => a.Id == assetId);
            if (asset != null)
            {
                asset.Status = inMaintenance ? "Maintenance" : "Available";
                asset.UpdatedDate = DateTime.Now;
                
                _db.AddAuditLog(assetId, "Asset", "Maintenance", inMaintenance ? "Sent to maintenance" : "Returned from maintenance");
                _notifications.AddActivity("Asset Maintenance Mode", $"Asset {assetId} marked {(inMaintenance ? "In Maintenance" : "Available")}.");
                _notifications.AddToast("Maintenance Status", $"Asset {assetId} is {(inMaintenance ? "now in Maintenance" : "Available again")}", inMaintenance ? ToastType.Warning : ToastType.Success);
            }
            return Task.CompletedTask;
        }
    }
}
