namespace NexAsset.Domain.Enums;

public enum ProcurementStatus
{
    Draft = 1,
    PendingApproval = 2,
    Approved = 3,
    Rejected = 4,
    Cancelled = 5,
    Ordered = 6
}

public enum StockMovementType
{
    StockIn = 1,
    StockOut = 2,
    Adjustment = 3,
    Reserved = 4,
    Released = 5
}

public enum MaintenanceType
{
    Preventive = 1,
    Corrective = 2
}

public enum MaintenanceStatus
{
    Requested = 1,
    Scheduled = 2,
    InProgress = 3,
    Completed = 4,
    Cancelled = 5
}

public enum TicketPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

public enum TicketStatus
{
    Open = 1,
    Assigned = 2,
    InProgress = 3,
    Resolved = 4,
    Closed = 5,
    Cancelled = 6
}

public enum NotificationType
{
    Info = 1,
    Warning = 2,
    Success = 3,
    Error = 4
}
