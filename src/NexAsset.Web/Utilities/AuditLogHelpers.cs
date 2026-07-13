using System;
using System.Collections.Generic;
using System.Linq;
using NexAsset.Web.Models.Mock;
using NexAsset.Web.Shared.Components;

namespace NexAsset.Web.Utilities
{
    /// <summary>
    /// Builds the <see cref="Timeline.TimelineEvent"/> list shown on entity detail pages.
    /// Extracted from the identical GetItemAuditEvents() method previously duplicated in
    /// AssetDetails.razor and EmployeeDetails.razor.
    /// </summary>
    public static class AuditLogHelpers
    {
        public static List<Timeline.TimelineEvent> BuildTimelineEvents(IEnumerable<AuditLogMock> auditLogs, string entityId)
        {
            var list = new List<Timeline.TimelineEvent>();

            foreach (var log in auditLogs.Where(l => l.EntityId == entityId))
            {
                list.Add(new Timeline.TimelineEvent
                {
                    Timestamp = log.Timestamp,
                    Title = log.Action.ToUpper() + " Event",
                    Details = log.Details,
                    User = log.User
                });
            }

            if (!list.Any())
            {
                list.Add(new Timeline.TimelineEvent
                {
                    Timestamp = DateTime.Now.AddMonths(-1),
                    Title = "RECORD CREATION",
                    Details = "Resource registered and integrated into NexAsset ledger.",
                    User = "System Seed"
                });
            }

            return list;
        }
    }
}
