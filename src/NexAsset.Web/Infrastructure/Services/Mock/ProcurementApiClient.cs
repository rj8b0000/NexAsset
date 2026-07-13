using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexAsset.Web.Models.Mock;
using NexAsset.Web.Infrastructure.Services;
using NexAsset.Web.State;

namespace NexAsset.Web.Infrastructure.Services.Mock
{
    /// <summary>
    /// In-memory implementation of <see cref="IProcurementApiClient"/> backed by <see cref="MockDatabaseService"/>.
    /// Temporary placeholder pending migration to the real NexAsset.API HTTP client.
    /// </summary>
    public class ProcurementApiClient : IProcurementApiClient
    {
        private readonly MockDatabaseService _db;
        private readonly NotificationState _notifications;

        public ProcurementApiClient(MockDatabaseService db, NotificationState notifications)
        {
            _db = db;
            _notifications = notifications;
        }

        public Task<List<ProcurementMock>> GetProcurementRequestsAsync()
        {
            return Task.FromResult(_db.ProcurementRequests.ToList());
        }

        public Task<ProcurementMock?> GetProcurementRequestAsync(string id)
        {
            return Task.FromResult(_db.ProcurementRequests.FirstOrDefault(r => r.Id == id));
        }

        public Task CreateProcurementRequestAsync(ProcurementMock pr)
        {
            pr.Id = $"PR-{_db.ProcurementRequests.Count + 101:D3}";
            pr.TotalValue = pr.Quantity * pr.UnitPrice;
            _db.ProcurementRequests.Insert(0, pr);

            _db.AddAuditLog(pr.Id, "Procurement", "Create", $"Created procurement request: {pr.ItemName}");
            _notifications.AddActivity("Procurement Created", $"New request {pr.Id} for {pr.ItemName} created by {pr.Requester}.");
            _notifications.AddToast("Procurement Request Sent", $"Successfully created request {pr.Id}", ToastType.Success);

            return Task.CompletedTask;
        }

        public Task UpdateProcurementRequestAsync(ProcurementMock pr)
        {
            var idx = _db.ProcurementRequests.FindIndex(r => r.Id == pr.Id);
            if (idx >= 0)
            {
                pr.TotalValue = pr.Quantity * pr.UnitPrice;
                _db.ProcurementRequests[idx] = pr;
                
                _db.AddAuditLog(pr.Id, "Procurement", "Update", $"Updated procurement: {pr.ItemName}");
                _notifications.AddActivity("Procurement Updated", $"Procurement request {pr.Id} was modified.");
                _notifications.AddToast("Procurement Updated", $"Updated procurement details for {pr.Id}", ToastType.Success);
            }
            return Task.CompletedTask;
        }

        public Task DeleteProcurementRequestAsync(string id)
        {
            var pr = _db.ProcurementRequests.FirstOrDefault(r => r.Id == id);
            if (pr != null)
            {
                _db.ProcurementRequests.Remove(pr);
                _db.AddAuditLog(id, "Procurement", "Delete", $"Deleted procurement request: {pr.ItemName}");
                _notifications.AddActivity("Procurement Request Deleted", $"Request {id} was permanently removed.");
                _notifications.AddToast("Request Removed", $"Deleted request {id}", ToastType.Warning);
            }
            return Task.CompletedTask;
        }

        public Task ApproveProcurementRequestAsync(string id)
        {
            var pr = _db.ProcurementRequests.FirstOrDefault(r => r.Id == id);
            if (pr != null)
            {
                pr.Status = "Approved";
                _db.AddAuditLog(id, "Procurement", "Approve", $"Approved purchase request for {pr.ItemName}");
                _notifications.AddActivity("Procurement Approved", $"Request {id} for {pr.ItemName} was approved.");
                _notifications.AddToast("Request Approved", $"Procurement request {id} is approved.", ToastType.Success);

                // Auto create corresponding asset (to mirror original codebase behaviour)
                var autoAsset = new AssetMock
                {
                    Id = $"AST-{_db.Assets.Count + 101:D3}",
                    Name = $"{pr.ItemName} (Procured)",
                    Category = "IT Equipment",
                    Serial = $"PROC-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                    Status = "Available",
                    Value = pr.UnitPrice,
                    Location = "HQ - New York",
                    AssignedTo = null,
                    PurchaseDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                _db.Assets.Insert(0, autoAsset);
                _db.AddAuditLog(autoAsset.Id, "Asset", "Create", $"Auto-created asset from procurement approval: {autoAsset.Name}");
                _notifications.AddActivity("Asset Auto-Created", $"Asset {autoAsset.Id} created from approved procurement PR.");
            }
            return Task.CompletedTask;
        }

        public Task RejectProcurementRequestAsync(string id)
        {
            var pr = _db.ProcurementRequests.FirstOrDefault(r => r.Id == id);
            if (pr != null)
            {
                pr.Status = "Rejected";
                _db.AddAuditLog(id, "Procurement", "Reject", $"Rejected purchase request for {pr.ItemName}");
                _notifications.AddActivity("Procurement Rejected", $"Request {id} for {pr.ItemName} was rejected.");
                _notifications.AddToast("Request Rejected", $"Procurement request {id} is rejected.", ToastType.Warning);
            }
            return Task.CompletedTask;
        }
    }
}
