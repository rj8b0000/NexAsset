using System;

namespace NexAsset.Web.State
{
    public class NavigationState
    {
        private readonly NotificationState _notificationState;

        public bool SidebarCollapsed { get; private set; } = false;

        // Active workspace selection. Ids reference real backend organizations/branches; the
        // names are kept alongside for display. Consumers (Dashboard, future org-scoped views)
        // read ActiveOrganizationId and subscribe to OnChange.
        public Guid? ActiveOrganizationId { get; private set; }
        public string ActiveOrganization { get; private set; } = "Select organization";
        public Guid? ActiveBranchId { get; private set; }
        public string ActiveBranch { get; private set; } = "All branches";

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

        /// <summary>Sets the active organization without announcing it (used for the initial default).</summary>
        public void InitializeOrganization(Guid id, string name)
        {
            if (ActiveOrganizationId is not null) return;
            ActiveOrganizationId = id;
            ActiveOrganization = name;
            NotifyStateChanged();
        }

        public void SwitchOrganization(Guid id, string name)
        {
            if (ActiveOrganizationId == id) return;
            ActiveOrganizationId = id;
            ActiveOrganization = name;
            // Branch belongs to the previous organization — reset it.
            ActiveBranchId = null;
            ActiveBranch = "All branches";
            _notificationState.AddActivity("Organization Switch", $"Switched active workspace to organization: {name}");
            _notificationState.AddToast("Organization Switched", $"Active workspace is now {name}", ToastType.Info);
            NotifyStateChanged();
        }

        public void SwitchBranch(Guid? id, string name)
        {
            ActiveBranchId = id;
            ActiveBranch = name;
            _notificationState.AddActivity("Branch Switch", $"Switched active operations branch to: {name}");
            _notificationState.AddToast("Branch Switched", $"Active branch is now {name}", ToastType.Info);
            NotifyStateChanged();
        }

        public void SetSearchQuery(string query)
        {
            SearchQuery = query;
            NotifyStateChanged();
        }
    }
}
