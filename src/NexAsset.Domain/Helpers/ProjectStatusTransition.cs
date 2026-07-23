using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Helpers;

public static class ProjectStatusTransition
{
    private static readonly IReadOnlyDictionary<ProjectStatus, HashSet<ProjectStatus>> AllowedTransitions =
        new Dictionary<ProjectStatus, HashSet<ProjectStatus>>
        {
            [ProjectStatus.Draft] = new() { ProjectStatus.Planning, ProjectStatus.Cancelled },
            [ProjectStatus.Planning] = new() { ProjectStatus.AwaitingApproval, ProjectStatus.Cancelled },
            [ProjectStatus.AwaitingApproval] = new() { ProjectStatus.Approved, ProjectStatus.Planning, ProjectStatus.Cancelled },
            [ProjectStatus.Approved] = new() { ProjectStatus.InProgress, ProjectStatus.Cancelled },
            [ProjectStatus.InProgress] = new() { ProjectStatus.OnHold, ProjectStatus.Completed, ProjectStatus.Cancelled },
            [ProjectStatus.OnHold] = new() { ProjectStatus.InProgress, ProjectStatus.Cancelled },
            [ProjectStatus.Completed] = new() { ProjectStatus.Archived },
            [ProjectStatus.Archived] = new() { ProjectStatus.InProgress },
            [ProjectStatus.Cancelled] = new HashSet<ProjectStatus>()
        };

    public static bool IsAllowed(ProjectStatus from, ProjectStatus to) =>
        AllowedTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);
}
