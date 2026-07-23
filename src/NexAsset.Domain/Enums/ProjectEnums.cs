namespace NexAsset.Domain.Enums;

public enum ProjectStatus
{
    Draft = 1,
    Planning = 2,
    AwaitingApproval = 3,
    Approved = 4,
    InProgress = 5,
    OnHold = 6,
    Completed = 7,
    Archived = 8,
    Cancelled = 9
}

public enum ProjectPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

public enum ParameterInputType
{
    Text = 1,
    Textarea = 2,
    Number = 3,
    Decimal = 4,
    Date = 5,
    Dropdown = 6,
    Checkbox = 7,
    Email = 8,
    Phone = 9,
    URL = 10
}

public enum TeamMemberStatus
{
    Active = 1,
    Released = 2
}

public enum AllocationStatus
{
    Active = 1,
    Returned = 2,
    PartiallyReturned = 3
}

public enum DocumentCategory
{
    Agreement = 1,
    Contract = 2,
    Survey = 3,
    BOQ = 4,
    TechnicalDrawing = 5,
    GovernmentApproval = 6,
    InspectionReport = 7,
    CompletionCertificate = 8,
    Invoice = 9,
    PurchaseDocument = 10,
    Photo = 11,
    Video = 12,
    Other = 13
}

public enum RiskCategory
{
    Technical = 1,
    Financial = 2,
    Operational = 3,
    External = 4,
    Regulatory = 5,
    Safety = 6,
    Resource = 7
}

public enum RiskProbability { Low = 1, Medium = 2, High = 3 }
public enum RiskImpact     { Low = 1, Medium = 2, High = 3 }
public enum RiskSeverity   { Low = 1, Medium = 2, High = 3, Critical = 4 }

public enum RiskStatus
{
    Open = 1,
    InProgress = 2,
    Mitigated = 3,
    Closed = 4
}

public enum TimelineEventType
{
    ProjectCreated = 1,
    PlanningStarted = 2,
    ApprovalRequested = 3,
    Approved = 4,
    TeamMemberAdded = 5,
    TeamMemberReleased = 6,
    AssetAllocated = 7,
    AssetReturned = 8,
    DocumentUploaded = 9,
    DocumentReplaced = 10,
    DocumentDeleted = 11,
    BudgetUpdated = 12,
    RiskAdded = 13,
    RiskClosed = 14,
    ParameterCreated = 15,
    ParameterUpdated = 16,
    ParameterDeleted = 17,
    ProjectCompleted = 18,
    ProjectArchived = 19,
    // Reserved for future modules
    TaskCreated = 100,
    TaskCompleted = 101,
    MilestoneReached = 102,
    GanttUpdated = 103,
    IssueOpened = 104,
    IssueClosed = 105,
    MeetingScheduled = 106,
    ProgressReportSubmitted = 107,
    RFIDScanRecorded = 108,
    IoTDataReceived = 109,
    AIRecommendationGenerated = 110
}

public enum ActivityType
{
    ProjectCreated = 1,
    ProjectUpdated = 2,
    StatusChanged = 3,
    PriorityChanged = 4,
    EmployeeAdded = 5,
    EmployeeReleased = 6,
    AssetAllocated = 7,
    AssetReturned = 8,
    DocumentUploaded = 9,
    DocumentReplaced = 10,
    DocumentDeleted = 11,
    ParameterCreated = 12,
    ParameterUpdated = 13,
    ParameterDeleted = 14,
    SectionCreated = 15,
    SectionDeleted = 16,
    BudgetUpdated = 17,
    RiskAdded = 18,
    RiskUpdated = 19,
    RiskClosed = 20
}

public enum NotificationPriority
{
    Info = 1,
    Warning = 2,
    Critical = 3
}

public enum ProjectNotificationType
{
    ProjectAssigned = 1,
    ApprovalRequested = 2,
    ApprovalCompleted = 3,
    TeamMemberAdded = 4,
    TeamMemberReleased = 5,
    AssetAllocated = 6,
    AssetReturned = 7,
    BudgetThresholdCrossed = 8,
    RiskCreated = 9,
    RiskCritical = 10,
    RiskClosed = 11,
    DocumentUploaded = 12,
    DocumentExpiring = 13,
    UpcomingDeadline = 14,
    ProjectCompleted = 15
}
