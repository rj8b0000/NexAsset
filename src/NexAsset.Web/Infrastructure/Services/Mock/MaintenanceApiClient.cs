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
    /// In-memory implementation of <see cref="IMaintenanceApiClient"/> backed by <see cref="MockDatabaseService"/>.
    /// Temporary placeholder pending migration to the real NexAsset.API HTTP client.
    /// </summary>
    public class MaintenanceApiClient : IMaintenanceApiClient
    {
        private readonly MockDatabaseService _db;
        private readonly NotificationState _notifications;

        public MaintenanceApiClient(MockDatabaseService db, NotificationState notifications)
        {
            _db = db;
            _notifications = notifications;
        }

        public Task<List<MaintenanceMock>> GetMaintenanceTicketsAsync()
        {
            return Task.FromResult(_db.MaintenanceTickets.ToList());
        }

        public Task<MaintenanceMock?> GetMaintenanceTicketAsync(string id)
        {
            return Task.FromResult(_db.MaintenanceTickets.FirstOrDefault(t => t.Id == id));
        }

        public Task CreateMaintenanceTicketAsync(MaintenanceMock ticket)
        {
            ticket.Id = $"MT-{_db.MaintenanceTickets.Count + 101:D3}";
            ticket.Status = "Open";
            _db.MaintenanceTickets.Insert(0, ticket);

            _db.AddAuditLog(ticket.Id, "Maintenance", "Create", $"Created maintenance ticket: {ticket.AssetName}");
            _notifications.AddActivity("Maintenance Ticket Opened", $"New maintenance ticket {ticket.Id} for {ticket.AssetName} was opened.");
            _notifications.AddToast("Ticket Opened", $"Successfully created ticket {ticket.Id}", ToastType.Success);

            return Task.CompletedTask;
        }

        public Task UpdateMaintenanceTicketAsync(MaintenanceMock ticket)
        {
            var idx = _db.MaintenanceTickets.FindIndex(t => t.Id == ticket.Id);
            if (idx >= 0)
            {
                _db.MaintenanceTickets[idx] = ticket;
                
                _db.AddAuditLog(ticket.Id, "Maintenance", "Update", $"Updated maintenance ticket: {ticket.AssetName}");
                _notifications.AddActivity("Maintenance Updated", $"Maintenance ticket {ticket.Id} details updated.");
                _notifications.AddToast("Maintenance Updated", $"Updated ticket details for {ticket.Id}", ToastType.Success);
            }
            return Task.CompletedTask;
        }

        public Task DeleteMaintenanceTicketAsync(string id)
        {
            var ticket = _db.MaintenanceTickets.FirstOrDefault(t => t.Id == id);
            if (ticket != null)
            {
                _db.MaintenanceTickets.Remove(ticket);
                _db.AddAuditLog(id, "Maintenance", "Delete", $"Deleted maintenance ticket: {ticket.AssetName}");
                _notifications.AddActivity("Maintenance Deleted", $"Ticket {id} was permanently removed.");
                _notifications.AddToast("Ticket Deleted", $"Deleted ticket {id}", ToastType.Warning);
            }
            return Task.CompletedTask;
        }

        public Task ResolveMaintenanceTicketAsync(string id)
        {
            var ticket = _db.MaintenanceTickets.FirstOrDefault(t => t.Id == id);
            if (ticket != null)
            {
                ticket.Status = "Resolved";
                _db.AddAuditLog(id, "Maintenance", "Resolve", $"Resolved maintenance ticket for {ticket.AssetName}");
                _notifications.AddActivity("Maintenance Resolved", $"Maintenance ticket {id} resolved.");
                _notifications.AddToast("Ticket Resolved", $"Maintenance ticket {id} marked Resolved.", ToastType.Success);

                // Find corresponding asset in mock database and toggle status if it matches name
                var asset = _db.Assets.FirstOrDefault(a => ticket.AssetName.Contains(a.Name) || a.Name.Contains(ticket.AssetName));
                if (asset != null && asset.Status == "Maintenance")
                {
                    asset.Status = "Available";
                    _db.AddAuditLog(asset.Id, "Asset", "Maintenance", "Returned from maintenance (auto-resolve)");
                    _notifications.AddToast("Asset Restored", $"Asset {asset.Id} is now Available again.", ToastType.Success);
                }
            }
            return Task.CompletedTask;
        }
    }
}
