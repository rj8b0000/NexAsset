using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.Models;
using NexAsset.Web.State;

namespace NexAsset.Web.Infrastructure.Services
{
    public class OrganizationApiClient : IOrganizationApiClient
    {
        private readonly MockDatabaseService _db;
        private readonly NotificationState _notifications;

        public OrganizationApiClient(MockDatabaseService db, NotificationState notifications)
        {
            _db = db;
            _notifications = notifications;
        }

        public Task<List<OrganizationMock>> GetOrganizationsAsync()
        {
            return Task.FromResult(_db.OrganizationDetails.ToList());
        }

        public Task<OrganizationMock?> GetOrganizationAsync(string id)
        {
            return Task.FromResult(_db.OrganizationDetails.FirstOrDefault(o => o.Id == id));
        }

        public Task CreateOrganizationAsync(OrganizationMock org)
        {
            org.Id = $"ORG-{_db.OrganizationDetails.Count + 101:D3}";
            org.CreatedDate = DateTime.Now;
            _db.OrganizationDetails.Insert(0, org);

            _db.AddAuditLog(org.Id, "Organization", "Create", $"Created organization profile: {org.Name}");
            _notifications.AddActivity("Organization Created", $"New organization profile for {org.Name} registered.");
            _notifications.AddToast("Organization Registered", $"Successfully registered organization {org.Id}", ToastType.Success);

            return Task.CompletedTask;
        }

        public Task UpdateOrganizationAsync(OrganizationMock org)
        {
            var idx = _db.OrganizationDetails.FindIndex(o => o.Id == org.Id);
            if (idx >= 0)
            {
                _db.OrganizationDetails[idx] = org;
                
                _db.AddAuditLog(org.Id, "Organization", "Update", $"Updated organization: {org.Name}");
                _notifications.AddActivity("Organization Updated", $"Organization profile {org.Id} details updated.");
                _notifications.AddToast("Organization Updated", $"Updated organization details for {org.Id}", ToastType.Success);
            }
            return Task.CompletedTask;
        }

        public Task DeleteOrganizationAsync(string id)
        {
            var org = _db.OrganizationDetails.FirstOrDefault(o => o.Id == id);
            if (org != null)
            {
                _db.OrganizationDetails.Remove(org);
                _db.AddAuditLog(id, "Organization", "Delete", $"Deleted organization: {org.Name}");
                _notifications.AddActivity("Organization Deleted", $"Organization profile {id} was permanently removed.");
                _notifications.AddToast("Organization Removed", $"Deleted organization {id}", ToastType.Warning);
            }
            return Task.CompletedTask;
        }
    }
}
