using System;
using NexAsset.Web.Infrastructure.Models;

namespace NexAsset.Web.State
{
    public class NavigationState
    {
        private readonly NotificationState _notificationState;

        public bool SidebarCollapsed { get; private set; } = false;
        public string ActiveOrganization { get; private set; } = "NexCorp Global";
        public string ActiveBranch { get; private set; } = "HQ - New York";
        public string SearchQuery { get; private set; } = "";

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public NavigationState(NotificationState notificationState)
        {
            _notificationState = notificationState;
        }

        public void SetSidebarCollapsed(bool collapsed)
        {
            SidebarCollapsed = collapsed;
            NotifyStateChanged();
        }

        public void SwitchOrganization(string org)
        {
            ActiveOrganization = org;
            _notificationState.AddActivity("Organization Switch", $"Switched active workspace to organization: {org}");
            _notificationState.AddToast("Organization Switched", $"Active workspace is now {org}", ToastType.Info);
            NotifyStateChanged();
        }

        public void SwitchBranch(string branch)
        {
            ActiveBranch = branch;
            _notificationState.AddActivity("Branch Switch", $"Switched active operations branch to: {branch}");
            _notificationState.AddToast("Branch Switched", $"Active branch is now {branch}", ToastType.Info);
            NotifyStateChanged();
        }

        public void SetSearchQuery(string query)
        {
            SearchQuery = query;
            NotifyStateChanged();
        }
    }
}
